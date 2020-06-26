using CT.KeyVault.Exceptions;
using CT.KeyVault.Models;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CT.KeyVault.Managers
{
    /// <summary>
    /// Manages connection and access to the KeyVault
    /// </summary>
    internal abstract class VaultManager : IVaultManager
    {
        /// <summary>
        /// Gets or sets the connection string to the keyvault
        /// </summary>
        protected string KeyvaultDns { get; set; }

        /// <inheritdoc/>
        public IKeyVaultClient Client { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VaultManager"/> class.
        /// </summary>
        /// <param name="dnsName">Connection string for the azure keyvault</param>
        protected VaultManager(string dnsName)
        {
            KeyvaultDns = dnsName;
        }

        /// <inheritdoc/>
        public string GetSecret(string secretName)
        {
            try
            {
                Task<string> taskResult = Task.Run<string>(async () => await this.GetSecretAsync(secretName).ConfigureAwait(false));
                taskResult.Wait();
                var secret = taskResult.Result;
                return secret;
            }
            catch (Exception ex)
            {
                throw new SecretException($"Unable to fetch secret '{secretName}' from the keyVault", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                SecretBundle bundle = await Client.GetSecretAsync(KeyvaultDns, secretName).ConfigureAwait(false);
                return bundle.Value;
            }
            catch (Exception ex)
            {
                throw new SecretException($"Unable to fetch secret '{secretName}' from the keyVault", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<List<SecretResultModel>> GetSecretsAsync(int? pageSize = null)
        {
            var secretsList = new List<SecretResultModel>();
            var secretResults = await Client.GetSecretsAsync(KeyvaultDns, pageSize).ConfigureAwait(false);

            foreach (var secret in secretResults)
            {
                var secretValue = await GetSecretAsync(secret.Identifier.Name).ConfigureAwait(false);
                secretsList.Add(new SecretResultModel { Id = secret.Id, Name = secret.Identifier.Name, Value = secretValue });
            }

            while (!string.IsNullOrEmpty(secretResults.NextPageLink))
            {
                secretResults = await Client.GetSecretsNextAsync(secretResults.NextPageLink).ConfigureAwait(false);
                foreach (var secret in secretResults)
                {
                    var secretValue = await this.GetSecretAsync(secret.Identifier.Name).ConfigureAwait(false);
                    secretsList.Add(new SecretResultModel { Id = secret.Id, Name = secret.Identifier.Name, Value = secretValue });
                }
            }

            return secretsList;
        }
    }
}