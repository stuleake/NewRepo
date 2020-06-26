using Api.FeeCalculator.Core;
using Api.FeeCalculator.Core.Helpers.JSEngine;
using AutoMapper;
using CT.KeyVault;
using Jint;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using TQ.Core;
using TQ.Core.Exceptions;
using TQ.Core.Filters;
using TQ.Core.Helpers;
using TQ.Data.FeeCalculator;

namespace Api.FeeCalculator
{
    /// <summary>
    /// Defining startup for Api.FeeCalculator
    /// </summary>

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class
        /// </summary>
        /// <param name="configuration">Application configuration object</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration settings
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container
        /// </summary>
        /// <param name="services">Service collection to configure services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var keyvaultObject = VaultManagerProvider.CreateKeyVaultManager(KeyVaultTypes.MSI, Configuration["KeyVault:BaseUrl"]);
            services.AddSingleton<IVaultManager>(keyvaultObject);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            var appId = keyvaultObject.GetSecret(Configuration["AzureAdB2C:AppId"]);
            var policy = keyvaultObject.GetSecret(Configuration["AzureAdB2C:SignInPolicy"]);
            var tenant = keyvaultObject.GetSecret(Configuration["AzureAdB2C:Tenant"]);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(jwtOptions =>
           {
               jwtOptions.Authority = $"https://{tenant}.b2clogin.com/{tenant}.onmicrosoft.com/{policy}/v2.0";
               jwtOptions.Audience = appId;
           });
            services.AddScoped<Engine>();
            services.AddScoped<JsEngine>();

            // Add logging for ILogger
            var appInsightsInstrumentationKey = keyvaultObject.GetSecret(Configuration["AppInsightsInstrumentationKey"]);
            services.AddLogging(builder => builder.AddApplicationInsights(appInsightsInstrumentationKey));
            services.AddApplicationInsightsTelemetry(appInsightsInstrumentationKey);

            var feeCalcConnection = keyvaultObject.GetSecret(Configuration["SqlConnection:FeeCalculator"]);
            services.AddDbContext<FeeCalculatorContext>(options => options.UseSqlServer(feeCalcConnection));
            services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.WithOrigins(Configuration["AllowedHosts"])));
            services.AddRazorPages();
            services.AddControllers(options => { options.Filters.Add(new CustomExceptionFilterAttribute()); });

            // Configure MediatR
            services.AddMediatR(TQAssemblies.AllAssemblies?.ToArray());

            // Configure swagger settings
            const string bearer = "Bearer";
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<SwaggerIgnoreFilter>();

                c.AddSecurityDefinition(bearer, new OpenApiSecurityScheme
                {
                    Description = $"JWT Authorization header using the {bearer} scheme. Example: \"Authorization: {bearer} {{token}}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = bearer
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = bearer
                            },
                            Scheme = "oauth2",
                            Name = bearer,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Terra Quest Fee Calculator API",
                    Description = "This API holds business logic related to the fee calculator"
                });

                var xmlFile = $"{typeof(Startup).Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Configure Health check settings
            AddHealthChecks(services, keyvaultObject);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">Application Builder object</param>
        /// <param name="env">Hosting Environment object</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseCors();
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

                    var operationId = context.Features.Get<Microsoft.ApplicationInsights.DataContracts.RequestTelemetry>().Context.Operation.Id;

                    await context.Response.WriteAsync(new TQ.Core.Models.ErrorDetails
                    {
                        OperationId = operationId,
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message
                    }.ToString()).ConfigureAwait(false);
                });
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            var healthCheckPath = Configuration["HealthChecks:BaseUrl"];
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Terra Quest FeeCalculator API");
            });
            app.UseHealthChecks(healthCheckPath, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = HealthCheck.CustomHealthCheckResponseWriterAsync
            });
        }

        private string GetConnectionString(string country, IVaultManager keyvault)
        {
            string connection = string.Empty;
            if (string.IsNullOrEmpty(country))
            {
                throw new TQException("Country Missing from Headers");
            }
            switch (country.ToUpper())
            {
                case "ENGLAND":
                    connection = keyvault.GetSecret(Configuration["SqlConnection:England-PlanningPortal"]);
                    break;

                case "WALES":
                    connection = keyvault.GetSecret(Configuration["SqlConnection:Wales-PlanningPortal"]);
                    break;
            }

            return connection;
        }

        private void AddHealthChecks(IServiceCollection services, IVaultManager keyvault)
        {
            // Publish health checks to Application Insights
            services.AddHealthChecks().AddApplicationInsightsPublisher(keyvault.GetSecret(Configuration["AppInsightsInstrumentationKey"]));

            // Configure Sql Server health checks
            services.AddHealthChecks()
                        .AddSqlServer(
                            connectionString: GetConnectionString("England", keyvault),
                            healthQuery: HealthCheck.SqlHealthcheckQuery,
                            name: HealthCheck.EnglishPlanningPortalSqlServerName,
                            failureStatus: HealthStatus.Unhealthy)
                        .AddSqlServer(
                            connectionString: GetConnectionString("Wales", keyvault),
                            healthQuery: HealthCheck.SqlHealthcheckQuery,
                            name: HealthCheck.WelshPlanningPortalSqlServerName,
                            failureStatus: HealthStatus.Unhealthy);

            // Configure Azure key vault health checks
            services.AddHealthChecks()
                .AddAzureKeyVault(
                    options => options.UseKeyVaultUrl(Configuration["KeyVault:BaseUrl"]),
                    name: HealthCheck.AzureKeyVaultName);

            // Configure Azure blob storage health checks
            services.AddHealthChecks().AddAzureBlobStorage(
                    keyvault.GetSecret(Configuration["StorageAccount"]),
                    name: HealthCheck.AzureStorageName,
                    failureStatus: HealthStatus.Unhealthy);
        }
    }
}