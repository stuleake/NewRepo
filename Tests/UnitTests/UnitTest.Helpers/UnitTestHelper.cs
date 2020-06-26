using Api.FeeCalculator.Core.Helpers.JSEngine;
using Api.Globals.Core.Helpers;
using AutoMapper;
using CT.KeyVault;
using CT.Storage;
using MediatR;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TQ.Core;
using TQ.Core.Helpers;
using TQ.Data.FeeCalculator;
using TQ.Data.FormEngine;
using TQ.Data.PlanningPortal;
using UnitTest.Helpers.FakeResources;

namespace UnitTest.Helpers
{
    /// <summary>
    /// Unit Test Helper
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class UnitTestHelper
    {
        /// <summary>
        /// Provides requested services to the requester
        /// </summary>
        /// <returns>A service provider to provide for the services</returns>
        public static ServiceProvider GiveServiceProvider()
        {
            var services = new ServiceCollection();

            // Configuration
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.AddSingleton<IConfiguration>(configuration);

            // Vault manager
            var vaultManager = SetupKeyVault();
            services.AddSingleton<IVaultManager>(vaultManager);

            // Storage Manager
            var storageManager = new FakeStorageManager();
            services.AddSingleton<IStorageManager>(storageManager);

            // Logger
            services.AddLogging();

            //// TODO - mock graph client
            services.AddScoped<B2CGraphClient>(c =>
            {
                return new B2CGraphClient(
                   "clientid", "clientsecret", "tenant");
            });
            var b2CObject = new B2CGraphClient(
               "clientid", "clientsecret", "tenant");

            services.AddSingleton<B2CGraphClient>(b2CObject);

            // PlanningPortal Context
            services.AddDbContextPool<PlanningPortalContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

            // FormsEngine Context
            services.AddDbContextPool<FormsEngineContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

            // Fee Calculator Context

            services.AddDbContextPool<FeeCalculatorContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

            var targetAssemblies = TQAssemblies.AllAssemblies;
            services.AddAutoMapper(targetAssemblies);
            services.AddMediatR(targetAssemblies?.ToArray());

            services.AddScoped<CeaserCipher>(c =>
            {
                return new CeaserCipher(Convert.ToInt32(configuration["SendGrid:Email-CipherSalt"]));
            });

            services.AddScoped<JsEngine>(engine =>
            {
                return new JsEngine(new Jint.Engine());
            });
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        /// <summary>
        /// Builds an in memory configuration collection based on the supplied settings
        /// </summary>
        /// <param name="inMemorySettings">the configuration settings</param>
        /// <returns>an in-memory representation of the configuration settings</returns>
        public static IConfiguration BuildConfiguration(Dictionary<string, string> inMemorySettings)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private static IVaultManager SetupKeyVault()
        {
            var secrets = new List<SecretBundle>
            {
                new SecretBundle { Id = "testkey1", Value = "testvalue1" },
                new SecretBundle { Id = "testkey2", Value = "testvalue2" },
                new SecretBundle { Id = "testkey3", Value = "testvalue3" },
                new SecretBundle { Id = "testkey4", Value = "testvalue4" },
                new SecretBundle { Id = "templateTestId-01", Value = "templateTestvalue4" },
                new SecretBundle { Id = "Email-ResetLinkTest", Value = "resetlinkTestvalue4" }
            };

            var secretItems = new List<SecretItem>
            {
                new SecretItem("https://teskeyvault/secrets/testkey1"),
                new SecretItem("https://teskeyvault/secrets/testkey2"),
                new SecretItem("https://teskeyvault/secrets/testkey3"),
                new SecretItem("https://teskeyvault/secrets/testkey4"),
                new SecretItem("https://teskeyvault/secrets/templateTestId-01"),
                new SecretItem("https://teskeyvault/secrets/Email-ResetLinkTest")
            };

            var vaultManager = VaultManagerProvider.CreateKeyVaultManager(KeyVaultTypes.MSI, "http://fakevault");
            vaultManager.Client = new FakeKeyVaultClient(secrets, secretItems).KeyVaultClient;
            return vaultManager;
        }
    }
}