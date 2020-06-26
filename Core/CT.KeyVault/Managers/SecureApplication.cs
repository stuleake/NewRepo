using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace CT.KeyVault.Managers
{
    /// <summary>
    /// Manages connection and access to the KeyVault via secure application
    /// </summary>
    internal sealed class SecureApplication : VaultManager
    {
        private static string ApplicationId { get; set; }

        private static string ApplicationAuthKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureApplication"/> class.
        /// </summary>
        /// <param name="dnsName">Connection string for the azure keyvault</param>
        /// <param name="applicationId">Application Id for the requesting application</param>
        /// <param name="applicationAuthKey">Application auth key of the requesting application</param>
        public SecureApplication(string dnsName, string applicationId, string applicationAuthKey) : base(dnsName)
        {
            Client = new KeyVaultClient(GetTokenAsync);
            ApplicationId = applicationId;
            ApplicationAuthKey = applicationAuthKey;
        }

        /// <summary>
        /// Asynchronously genrate the auth token required for access
        /// </summary>
        /// <param name="authority">Auhtority</param>
        /// <param name="resource">Resource</param>
        /// <param name="scope">Scope</param>
        /// <returns>A <see cref="string"/> for the auth token</placeholder></returns>
        public static async Task<string> GetTokenAsync(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(ApplicationId, ApplicationAuthKey);
            AuthenticationResult authToken = await context.AcquireTokenAsync(resource, clientCred).ConfigureAwait(false);

            if (authToken == null)
            {
                throw new SetupException("Initialization error");
            }

            return authToken.AccessToken;
        }
    }
}