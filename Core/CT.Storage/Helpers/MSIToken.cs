using Microsoft.Azure.Services.AppAuthentication;

namespace CT.Storage.Helpers
{
    /// <summary>
    /// Get token for MSI Connection
    /// </summary>
    internal static class MsiToken
    {
        /// <summary>
        /// Generate the MSI token
        /// </summary>
        /// <returns>A string for the token</returns>
        public static string GetMsiToken()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            string accessToken = azureServiceTokenProvider.GetAccessTokenAsync("https://management.azure.com/").GetAwaiter().GetResult();
            return accessToken;
        }
    }
}