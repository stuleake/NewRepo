using CT.KeyVault;
using CT.KeyVault.Exceptions;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnitTest.Helpers.FakeResources;
using Xunit;

namespace UnitTest.CT.KeyVault.Managers
{
    [ExcludeFromCodeCoverage]
    public class SecureApplicationTest
    {
        private readonly IConfigurationRoot config;

        public SecureApplicationTest()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        private IVaultManager SetupVaultManager()
        {
            var secrets = new List<SecretBundle>
            {
                new SecretBundle { Id = "testkey1", Value = "testvalue1" },
                new SecretBundle { Id = "testkey2", Value = "testvalue2" },
                new SecretBundle { Id = "testkey3", Value = "testvalue3" },
                new SecretBundle { Id = "testkey4", Value = "testvalue4" }
            };

            var secretItems = new List<SecretItem>
            {
                new SecretItem("https://teskeyvault/secrets/testkey1"),
                new SecretItem("https://teskeyvault/secrets/testkey2"),
                new SecretItem("https://teskeyvault/secrets/testkey3"),
                new SecretItem("https://teskeyvault/secrets/testkey4")
            };

            var vaultMgr = VaultManagerProvider.CreateKeyVaultManager(KeyVaultTypes.Application, config["KeyVaultURL"], string.Empty, string.Empty);
            vaultMgr.Client = new FakeKeyVaultClient(secrets, secretItems).KeyVaultClient;

            return vaultMgr;
        }

        [Theory]
        [InlineData("testkey1", "testvalue1")]
        [InlineData("testkey2", "testvalue2")]
        [InlineData("testkey3", "testvalue3")]
        [InlineData("testkey4", "testvalue4")]
        public void TestGetSecret(string keyname, string expectedValue)
        {
            // Arrange
            var vaultMgr = SetupVaultManager();

            // Act
            var data = vaultMgr.GetSecret(keyname);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(expectedValue, data);
        }

        [Theory]
        [InlineData(1)]
        public void TestGetSecrets(int maxResults)
        {
            // Arrange
            var vaultMgr = SetupVaultManager();

            // Act
            var dataList = vaultMgr.GetSecretsAsync(maxResults).Result;

            // Assert
            Assert.Equal(4, dataList.Count);
        }

        [Fact]
        public void TestGetAllSecrets()
        {
            // Arrange
            var vaultMgr = SetupVaultManager();

            // Act
            var dataList = vaultMgr.GetSecretsAsync().Result;

            // Assert
            Assert.NotNull(dataList);
            Assert.Equal(4, dataList.Count);
        }

        [Theory]
        [InlineData("notexistedkey")]
        public void TestGetSecretIfNotExists(string keyname)
        {
            // Arrange
            var vaultMgr = SetupVaultManager();

            // Act
            string data = null;
            Exception exceptionData = null;
            try
            {
                data = vaultMgr.GetSecret(keyname);
            }
            catch (Exception ex)
            {
                exceptionData = ex;
            }

            // Assert
            Assert.Null(data);
            Assert.NotNull(exceptionData);
            Assert.IsType<SecretException>(exceptionData);
        }

        [Fact]
        public void SetupExceptionTest()
        {
            // Arrange
            var setupException = new SetupException("SetupException");

            // Assert
            Assert.NotNull(setupException);
            Assert.True(string.Compare(setupException.Message, "SetupException", true) == 0);
        }
    }
}