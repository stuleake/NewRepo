using Api.Globals.Core.Helpers;
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
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using TQ.Core;
using TQ.Core.Enums;
using TQ.Core.Filters;
using TQ.Core.Helpers;

namespace Api.Globals
{
    /// <summary>
    /// To configure services and app request pipeline
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="Startup"/> class
        /// </summary>
        /// <param name="configuration">object of configuration being passed using dependency injection</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration settings
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container
        /// </summary>
        /// <param name="services">The service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Register the keyvault object.
            var keyvaultObject = VaultManagerProvider.CreateKeyVaultManager(KeyVaultTypes.MSI, Configuration["KeyVault:BaseUrl"]);
            services.AddSingleton<IVaultManager>(keyvaultObject);

            // Configure Automapper
            services.AddAutoMapper(TQAssemblies.AllAssemblies);

            var appId = keyvaultObject.GetSecret(this.Configuration["AzureAdB2C:AppId"]);
            var policy = keyvaultObject.GetSecret(this.Configuration["AzureAdB2C:SignInPolicy"]);
            var tenant = keyvaultObject.GetSecret(this.Configuration["AzureAdB2C:Tenant"]);

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

            // Add cors configuration
            services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.WithOrigins(this.Configuration["AllowedHosts"])));

            // Add logging for ILogger
            var appInsightsInstrumentationKey = keyvaultObject.GetSecret(Configuration["AppInsightsInstrumentationKey"]);
            services.AddLogging(builder => builder.AddApplicationInsights(appInsightsInstrumentationKey));
            services.AddApplicationInsightsTelemetry(appInsightsInstrumentationKey);

            services.AddRazorPages();
            services.AddControllers(options => { options.Filters.Add(new CustomExceptionFilterAttribute()); });

            // Add the base email services
            services.AddScoped<EmailService>(c =>
            {
                return new EmailService(keyvaultObject.GetSecret(this.Configuration["SendGrid:AppKey"]));
            });
            services.AddScoped<B2CGraphClient>(c =>
            {
                return new B2CGraphClient(
                    keyvaultObject.GetSecret(Configuration["ClientId"]),
                    keyvaultObject.GetSecret(Configuration["ClientSecret"]),
                    keyvaultObject.GetSecret(Configuration["Tenant"]));
            });

            services.AddScoped<AzureUser>(c =>
            {
                return new AzureUser();
            });
            services.AddScoped<Global>(c =>
            {
                return new Global();
            });
            services.AddScoped<AzureMapper>(c =>
            {
                return new AzureMapper();
            });

            services.AddSingleton<AzureMapper>();
            services.AddScoped<CeaserCipher>(cipher =>
            {
                return new CeaserCipher(Convert.ToInt32(keyvaultObject.GetSecret(Configuration["SendGrid:Email-CipherSalt"])));
            });

            // Register the Blob Manager for accessing storage account.
            var connection = keyvaultObject.GetSecret(Configuration["StorageAccount"]);
            var storageManager = StorageProvider.CreateManager(connection, ConnectionTypes.ConnectionString, 60);
            services.AddSingleton<IStorageManager>(storageManager);

            // Configure MediatR
            services.AddMediatR(TQAssemblies.AllAssemblies?.ToArray());

            // Configure API versioning
            //services.AddApiVersioning(o => o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0));
           
            





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
                    Title = "API.Globals API V1",
                    Description = "Version 1 of the API.Globals API"
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
        /// <param name="app">Reference of the application builder to which DI is being made</param>
        /// <param name="env">Environment</param>
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
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "API.Globals API V1");
            });
        }

        private void AddHealthChecks(IServiceCollection services, IVaultManager keyvault)
        {
            // Publish health checks to Application Insights
            services.AddHealthChecks().AddApplicationInsightsPublisher(keyvault.GetSecret(Configuration["AppInsightsInstrumentationKey"]));

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