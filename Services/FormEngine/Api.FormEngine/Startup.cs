using Api.FormEngine.Core.PipelineBehaviours;
using Api.FormEngine.Core.Services.Globals;
using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.Services.Processors;
using Api.FormEngine.Core.Services.Repository;
using Api.FormEngine.Core.Services.Validators;
using Api.FormEngine.Core.ViewModels.SheetModels;
using AutoMapper;
using CT.KeyVault;
using CT.Storage;
using FluentValidation;
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
using TQ.Core.Enums;
using TQ.Core.Exceptions;
using TQ.Core.Filters;
using TQ.Core.Helpers;
using TQ.Core.Repository;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine
{
    /// <summary>
    /// Defining startup for Api.FormEngine
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

            // Configure Vault Manager
            var keyvaultObject = VaultManagerProvider.CreateKeyVaultManager(KeyVaultTypes.MSI, Configuration["KeyVault:BaseUrl"]);
            services.AddSingleton<IVaultManager>(keyvaultObject);

            // Configure Storage Provider
            var storageConnectionString = keyvaultObject.GetSecret(Configuration["StorageAccount"]);
            var sotrageManager = StorageProvider.CreateManager(storageConnectionString, CT.Storage.Enum.ConnectionTypes.ConnectionString, 60);
            services.AddSingleton<IStorageManager>(sotrageManager);

            // Configure Automapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Core.MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

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

            var frmEngineConnection = keyvaultObject.GetSecret(Configuration["SqlConnection:FormEngineDB"]);
            services.AddDbContext<FormsEngineContext>(options => options.UseSqlServer(frmEngineConnection));

            services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.WithOrigins(Configuration["AllowedHosts"])));
            services.AddRazorPages();
            services.AddControllers(options => { options.Filters.Add(new CustomExceptionFilterAttribute()); });


            // Parsing Services
            services.AddScoped<Core.Services.Validators.IValidator<ExcelSheetsData>, QuestionSetValidator>();
            services.AddScoped<IProcessor<IEnumerable<Aggregations>>, FieldAggregationsProcessor>();
            services.AddScoped<IProcessor<IEnumerable<Core.ViewModels.SheetModels.Section>>, SectionFieldMappingProcessor>();
            services.AddScoped<IProcessor<FieldConstraintParserModel>, FieldConstraintProcessor>();
            services.AddScoped<IProcessor<QuestionSetParserModel>, QuestionSetProcessor>();
            services.AddScoped<IProcessor<SectionParserModel>, SectionProcessor>();
            services.AddScoped<IProcessor<SectionMappingParserModel>, SectionMappingProcessor>();
            services.AddScoped<IProcessor<FieldParserModel>, FieldProcessor>();
            services.AddScoped<IProcessor<TaxonomyParserModel>, TaxonomyProcessor>();

            services.AddScoped<IParser<IEnumerable<Aggregations>, IEnumerable<FieldAggregation>>, FieldAggregationsParser>();
            services.AddScoped<IParser<Core.ViewModels.SheetModels.Section, IEnumerable<SectionFieldMapping>>, SectionFieldMappingParser>();
            services.AddScoped<IParser<FieldConstraintParserModel, IEnumerable<FieldConstraint>>, FieldConstraintParser>();
            services.AddScoped<IParser<QuestionSetParserModel, QS>, QuestionSetParser>();
            services.AddScoped<IParser<SectionMappingParserModel, IEnumerable<QSSectionMapping>>, SectionMappingParser>();
            services.AddScoped<IParser<SectionParserModel, IEnumerable<TQ.Data.FormEngine.Schemas.Forms.Section>>, SectionParser>();
            services.AddScoped<IParser<FieldParserModel, IEnumerable<TQ.Data.FormEngine.Schemas.Forms.Field>>, FieldParser>();
            services.AddScoped<IParser<TaxonomyParserModel, IEnumerable<Taxonomy>>, TaxonomyParser>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));

            // Configure Fluent Validation
            services.AddValidatorsFromAssemblies(TQAssemblies.AllAssemblies?.ToArray(), ServiceLifetime.Transient);

            // Configure MediatR
            services.AddMediatR(TQAssemblies.AllAssemblies?.ToArray());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddHttpClient<IGlobalClient, GlobalsClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration["ApiUri:Globals:BaseUrl"]);
            });

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
                    Title = "Terra Quest Forms Engine API",
                    Description = "This API holds business logic related to the form engine",
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Terra Quest Froms Engine API");
            });
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
                           failureStatus: HealthStatus.Unhealthy)
                       .AddSqlServer(
                           connectionString: keyvault.GetSecret(Configuration["SqlConnection:FormEngineDB"]),
                           healthQuery: HealthCheck.SqlHealthcheckQuery,
                           name: HealthCheck.FormsEngineSqlServerName,
                           failureStatus: HealthStatus.Unhealthy);

            // Configure Azure key vault health checks
            services.AddHealthChecks().AddAzureKeyVault(
                            options => options.UseKeyVaultUrl(Configuration["KeyVault:BaseUrl"]),
                            name: HealthCheck.AzureKeyVaultName);

            // Configure Azure blob storage health checks
            services.AddHealthChecks().AddAzureBlobStorage(
                    keyvault.GetSecret(Configuration["StorageAccount"]),
                    name: HealthCheck.AzureStorageName,
                    failureStatus: HealthStatus.Unhealthy);
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
    }
}