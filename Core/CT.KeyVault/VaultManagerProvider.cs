using CT.KeyVault.Managers;

namespace CT.KeyVault
{
    /// <summary>
    /// Factory to create KeyVault Managersz
    /// </summary>
    public static class VaultManagerProvider
    {
        /// <summary>
        /// Geneate a Key Valut Manager
        /// </summary>
        /// <param name="type">The type of manager</param>
        /// <param name="vaultDnsName">The connection string to the key vault</param>
        /// <param name="applicationId">The application id of the requesting application</param>
        /// <param name="applicationAuthKey">The application auth key of the requesting application</param>
        /// <returns>A <see cref="IVaultManager"/> of the requested type</placeholder></returns>
        public static IVaultManager CreateKeyVaultManager(KeyVaultTypes type, string vaultDnsName, string applicationId = "", string applicationAuthKey = "")
        {
            switch (type)
            {
                case KeyVaultTypes.MSI:
                    return new ManagedServiceIdentity(vaultDnsName);

                case KeyVaultTypes.Application:
                    return new SecureApplication(vaultDnsName, applicationId, applicationAuthKey);

                default:
                    return new ManagedServiceIdentity(vaultDnsName);
            }
        }
    }
}