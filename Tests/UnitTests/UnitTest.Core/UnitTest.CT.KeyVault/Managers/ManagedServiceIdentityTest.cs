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
    /// <summary>
    /// MSI Test
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ManagedServiceIdentityTest
    {
        private readonly IConfigurationRoot config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedServiceIdentityTest"/> class.
        /// </summary>
        public ManagedServiceIdentityTest()
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

            var vaultMgr = VaultManagerProvider.CreateKeyVaultManager(KeyVaultTypes.MSI, config["KeyVaultURL"]);
            vaultMgr.Client = new FakeKeyVaultClient(secrets, secretItems).KeyVaultClient;

            return vaultMgr;
        }

        /// <summary>
        /// Test the keys that exist
        /// </summary>
        /// <param name="keyname">Keyname</param>
        /// <param name="expectedvalue">Expected Value</param>
        [Theory]
        [InlineData("testkey1", "testvalue1")]
        [InlineData("testkey2", "testvalue2")]
        [InlineData("testkey3", "testvalue3")]
        [InlineData("testkey4", "testvalue4")]
        public void TestGetSecretIFExists(string keyname, string expectedvalue)
        {
            // Arrange
            var vaultMgr = SetupVaultManager();

            // Act
            var data = vaultMgr.GetSecret(keyname);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(expectedvalue, data);
        }

        /// <summary>
        /// Test fetch all secrets
        /// </summary>
        /// <param name="expectedcount">Expected Count</param>
        [Theory]
        [InlineData(1)]
        public void TestGetSecrets(int expectedcount)
        {
            // Arrange
            var vaultMgr = SetupVaultManager();

            // Act
            var dataList = vaultMgr.GetSecretsAsync(expectedcount).Result;

            // Assert
            Assert.NotNull(dataList);
            Assert.Equal(4, dataList.Count);
        }

        /// <summary>
        /// Test get all secrets
        /// </summary>
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

        /// <summary>
        /// Test secrets that do not exist
        /// </summary>
        /// <param name="keyname">Keyname</param>
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

        /// <summary>
        /// Test Setup exception
        /// </summary>
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