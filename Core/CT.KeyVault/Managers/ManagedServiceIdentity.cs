using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace CT.KeyVault.Managers
{
    /// <summary>
    /// Manages connection and access to the KeyVault via MSI - ManagedServiceIdentity
    /// </summary>
    internal sealed class ManagedServiceIdentity : VaultManager
    {
        /// <inheritdoc/>
        public ManagedServiceIdentity(string dnsName) : base(dnsName)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            Client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        }
    }
}