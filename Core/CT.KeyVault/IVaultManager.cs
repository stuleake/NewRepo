using CT.KeyVault.Models;
using Microsoft.Azure.KeyVault;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CT.KeyVault
{
    /// <summary>
    /// Interface for the actions available on the vault manager
    /// </summary>
    public interface IVaultManager
    {
        /// <summary>
        /// Gets or sets the keyvault client
        /// </summary>
        IKeyVaultClient Client { get; set; }

        /// <summary>
        /// Fetch a secret for a paricular key from the key vault
        /// </summary>
        /// <param name="secretName">Name of the secret to be fetched</param>
        /// <returns>Value for the secret from the key valut</returns>
        string GetSecret(string secretName);

        /// <summary>
        /// Asynchronously fetch a secret for a paricular key from the key vault
        /// </summary>
        /// <param name="secretName">Name of the secret to be fetched</param>
        /// <returns>Value for the secret from the key valut</returns>
        Task<string> GetSecretAsync(string secretName);

        /// <summary>
        /// Asynchronously fetch all secrets from the key vault
        /// </summary>
        /// <param name="pageSize">Page size to fetch keys from the keyvault in single iteration</param>
        /// <returns>A list for all the keys available in the keyvalt</returns>
        Task<List<SecretResultModel>> GetSecretsAsync(int? pageSize = null);
    }
}