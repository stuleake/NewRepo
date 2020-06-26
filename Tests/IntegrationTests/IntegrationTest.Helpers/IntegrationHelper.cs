using Api.Globals.Core.Helpers;
using Api.Planner.Core.Services.Globals;
using AutoMapper;
using CT.KeyVault;
using CT.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TQ.Core;
using TQ.Core.Helpers;
using TQ.Data.FormEngine;
using TQ.Data.PlanningPortal;

namespace IntegrationTest.Helpers
{
    /// <summary>
    /// Integration Helper class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IntegrationHelper
    {
        /// <summary>
        /// Provides requested services to the requester
        /// </summary>
        /// <returns>A service provider to provide for the services</returns>
        public static ServiceProvider GiveServiceProvider()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);

            var keyvaultObject = VaultManagerProvider.CreateKeyVaultManager(KeyVaultTypes.MSI, configuration["KeyVault:BaseUrl"]);
            services.AddSingleton<IVaultManager>(keyvaultObject);

            var frmEngineConnection = keyvaultObject.GetSecret(configuration["SqlConnection:FormEngineDB"]);
            services.AddDbContext<FormsEngineContext>(options => options.UseSqlServer(frmEngineConnection));

            var planningPortalConnection = keyvaultObject.GetSecret(configuration["SqlConnection:England-PlanningPortal"]);
            services.AddDbContext<PlanningPortalContext>(options => options.UseSqlServer(planningPortalConnection));

            var targetAssemblies = TQAssemblies.AllAssemblies;
            services.AddAutoMapper(targetAssemblies);
            services.AddMediatR(targetAssemblies?.ToArray());

            var storageConnection = keyvaultObject.GetSecret(configuration["StorageAccount"]);
            var storageManager = StorageProvider.CreateManager(storageConnection, CT.Storage.Enum.ConnectionTypes.ConnectionString, 60);
            services.AddSingleton<IStorageManager>(storageManager);

            var appInsightsInstrumentationKey = keyvaultObject.GetSecret(configuration["AppInsightsInstrumentationKey"]);
            services.AddLogging(builder => builder.AddApplicationInsights(appInsightsInstrumentationKey));

            services.AddSingleton<AzureMapper>();

            services.AddScoped<B2CGraphClient>(c =>
            {
                return new B2CGraphClient(
                    keyvaultObject.GetSecret(configuration["ClientId"]),
                    keyvaultObject.GetSecret(configuration["ClientSecret"]),
                    keyvaultObject.GetSecret(configuration["Tenant"]));
            });
            var b2CObject = new B2CGraphClient(
                keyvaultObject.GetSecret(configuration["ClientId"]),
                keyvaultObject.GetSecret(configuration["ClientSecret"]),
                keyvaultObject.GetSecret(configuration["Tenant"]));

            services.AddSingleton<B2CGraphClient>(b2CObject);
            services.AddScoped<CeaserCipher>(cipher =>
            {
                return new CeaserCipher(Convert.ToInt32(keyvaultObject.GetSecret(configuration["SendGrid:Email-CipherSalt"])));
            });
            services.AddHttpClient<IGlobalsClient, GlobalsClient>(c =>
            {
                c.BaseAddress = new Uri(configuration["ApiUri:Globals:BaseUrl"]);
            });
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}