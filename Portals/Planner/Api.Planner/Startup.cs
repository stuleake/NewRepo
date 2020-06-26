using Api.Planner.Core.Commands.AddressSearch;
using Api.Planner.Core.Services.AddressSearch;
using Api.Planner.Core.Services.FormEngine;
using Api.Planner.Core.Services.Gatsby;
using Api.Planner.Core.Services.Globals;
using Api.Planner.Core.Services.PP2;
using AutoMapper;
using CT.KeyVault;
using CT.Storage;
using CT.Storage.Enum;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.Reflection;
using TQ.Core;
using TQ.Core.Enums;
using TQ.Core.Filters;
using TQ.Core.Helpers;

namespace Api.Planner
{
    /// <summary>
    /// Defining startup for Api.Admin
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
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
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection to configure services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Register the keyvault object.
            var keyvaultObject = VaultManagerProvider.CreateKeyVaultManager(KeyVaultTypes.MSI, Configuration["KeyVault:BaseUrl"]);
            services.AddSingleton<IVaultManager>(keyvaultObject);

            // Register the Blob Manager for accessing storage account.
            var connection = keyvaultObject.GetSecret(Configuration["StorageAccount"]);
            var storageManager = StorageProvider.CreateManager(connection, ConnectionTypes.ConnectionString, 60);
            services.AddSingleton<IStorageManager>(storageManager);

            // Configure Automapper
            services.AddAutoMapper(TQAssemblies.AllAssemblies);

            var appId = keyvaultObject.GetSecret(Configuration["AzureAdB2C:AppId"]);
            var policy = keyvaultObject.GetSecret(Configuration["AzureAdB2C:SignInPolicy"]);
            var tenant = keyvaultObject.GetSecret(Configuration["AzureAdB2C:Tenant"]);

            // Add authentication mechanism.
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(jwtOptions =>
           {
               jwtOptions.Authority = $"https://{tenant}.b2clogin.com/{tenant}.onmicrosoft.com/{policy}/v2.0";
               jwtOptions.Audience = appId;
           });

            // Add logging for ILogger
            var appInsightsInstrumentationKey = keyvaultObject.GetSecret(Configuration["AppInsightsInstrumentationKey"]);
            services.AddLogging(builder => builder.AddApplicationInsights(appInsightsInstrumentationKey));
            services.AddApplicationInsightsTelemetry(appInsightsInstrumentationKey);

            // Add cors configuration
            services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.WithOrigins(Configuration["AllowedHosts"])));
            services.AddHttpClient<IFormEngineClient, FormEngineClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration["ApiUri:FormEngine:BaseUrl"]);
            });
            services.AddHttpClient<IPP2HttpClient, PP2HttpClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration["ApiUri:PP2:BaseUrl"]);
            });
            services.AddScoped<B2CGraphClient>(c =>
            {
                return new B2CGraphClient(
                    keyvaultObject.GetSecret(Configuration["ClientId"]),
                    keyvaultObject.GetSecret(Configuration["ClientSecret"]),
                    keyvaultObject.GetSecret(Configuration["Tenant"]));
            });
            var b2CObject = new B2CGraphClient(
                keyvaultObject.GetSecret(Configuration["ClientId"]),
                keyvaultObject.GetSecret(Configuration["ClientSecret"]),
                keyvaultObject.GetSecret(Configuration["Tenant"]));

            services.AddSingleton<B2CGraphClient>(b2CObject);
            services.AddHttpClient<IGlobalsClient, GlobalsClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration["ApiUri:Globals:BaseUrl"]);
            });

            services.AddHttpClient<IGatsbyClient, GatsbyClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration["GatsBy:BaseUrl"]);
            });

            services.AddRazorPages();
            services.AddControllers(options => { options.Filters.Add(new CustomExceptionFilterAttribute()); });

            // Configure MediatR
            services.AddMediatR(TQAssemblies.AllAssemblies?.ToArray());

            // Configure MediatR Geocoding Services
            services.AddMediatR(typeof(GetSimpleAddressByPostcodeCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetFullAddressByPostcodeCommand).GetTypeInfo().Assembly);
            services.AddScoped(typeof(IGeocodingClient), typeof(GeocodingClient));
            services.AddHttpClient<IGeocodingClient, GeocodingClient>();

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
                    Title = "Terra Quest Planner API",
                    Description = "This API holds business logic related to the planner app"
                });

                var xmlFile = $"{typeof(Startup).Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Configure Health check settings
            AddHealthChecks(services, keyvaultObject);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            var healthCheckPath = Configuration["HealthChecks:BaseUrl"];
            app.UseHealthChecks(healthCheckPath, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = HealthCheck.CustomHealthCheckResponseWriterAsync
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks(healthCheckPath).RequireAuthorization(new CustomAuthorizeAttribute(new RoleTypes[] { RoleTypes.PP2SuperAdmin, RoleTypes.SupportUser }));
            });
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Terra Quest Planner API");
            });
        }

        private void AddHealthChecks(IServiceCollection services, IVaultManager keyvault)
        {
            // Publish health checks to Application Insights
            services.AddHealthChecks().AddApplicationInsightsPublisher(keyvault.GetSecret(Configuration["AppInsightsInstrumentationKey"]));

            // Configure Sql Server health checks
            services.AddHealthChecks()
                       .AddSqlServer(
                           connectionString: keyvault.GetSecret(Configuration["SqlConnection:FormEngineDB"]),
                           healthQuery: HealthCheck.SqlHealthcheckQuery,
                           name: HealthCheck.FormsEngineSqlServerName,
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