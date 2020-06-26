using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Rest.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace UnitTest.Helpers.FakeResources
{
    /// <summary>
    /// Fake keyvault client
    /// </summary>
    public sealed class FakeKeyVaultClient
    {
        private readonly IEnumerable<SecretBundle> secrets;
        private readonly IEnumerable<SecretItem> secretItems;

        /// <summary>
        /// Gets the mocked key vault client
        /// </summary>
        public IKeyVaultClient KeyVaultClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeKeyVaultClient"/> class.
        /// </summary>
        /// <param name="secrets">The secret bundle</param>
        /// <param name="secretItems">The secret items</param>
        public FakeKeyVaultClient(IEnumerable<SecretBundle> secrets, IEnumerable<SecretItem> secretItems)
        {
            KeyVaultClient = Mock.Create<IKeyVaultClient>();

            // Initialize secrets and secretItems
            this.secretItems = secretItems;
            this.secrets = secrets;

            // Setup the methods required
            KeyVaultClient.Arrange(client => KeyVaultClient.GetSecretWithHttpMessagesAsync(
                Arg.IsAny<string>(),
                Arg.IsAny<string>(),
                Arg.IsAny<string>(),
                Arg.IsAny<Dictionary<string, List<string>>>(),
                Arg.IsAny<CancellationToken>())).Returns((
                    string vaultBaseUrl,
                    string secretName,
                    string secretVersion,
                    Dictionary<string,
                    List<string>> customHeaders,
                    CancellationToken cancellationToken) =>
                {
                    return GetSecretAsync(secretName);
                });

            KeyVaultClient.Arrange(client =>
            KeyVaultClient.GetSecretsWithHttpMessagesAsync(Arg.IsAny<string>(), Arg.IsAny<int?>(), Arg.IsAny<Dictionary<string, List<string>>>(), Arg.IsAny<CancellationToken>()))
            .Returns((string vaultBaseUrl, int? maxResults, Dictionary<string, List<string>> customHeaders, CancellationToken cancellationToken) => GetSecretsAsync(maxResults ?? 25));

            KeyVaultClient.Arrange(client => KeyVaultClient.GetSecretsNextWithHttpMessagesAsync(Arg.IsAny<string>(), Arg.IsAny<Dictionary<string, List<string>>>(), Arg.IsAny<CancellationToken>()))
            .Returns((string nextPageLink, Dictionary<string, List<string>> customHeaders, CancellationToken cancellationToken) =>
            {
                var inputs = nextPageLink.Split('|').Select(inp => int.Parse(inp));
                return GetSecretsAsync(inputs.ElementAt(0), inputs.ElementAt(1));
            });
        }

        /// <summary>
        /// Method to get the AzureOperationResponse for the single secret
        /// </summary>
        /// <param name="secretName">The name of the secret key</param>
        /// <returns>A string value stored in the key</returns>
        public async Task<AzureOperationResponse<SecretBundle>> GetSecretAsync(string secretName)
        {
            var secret = secrets.FirstOrDefault(s => s.Id == secretName);
            var result = new AzureOperationResponse<SecretBundle> { Body = secret, Response = new System.Net.Http.HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK } };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        /// <summary>
        /// Method to get the azureoperationresponse for all secrets/// </summary>
        /// <param name="maxResults">Max results that can be returned</param>
        /// <param name="nextLink">Link to fetch the next maxItems</param>
        /// <returns>A list of secrect vaules from the vault</returns>
        public async Task<AzureOperationResponse<IPage<SecretItem>>> GetSecretsAsync(int? maxResults = 25, int nextLink = -1)
        {
            var pages = Mock.Create<IPage<SecretItem>>();

            if (maxResults == null || (int)maxResults > secretItems.Count())
            {
                pages.Arrange(page => page.NextPageLink).Returns(string.Empty);
                pages.Arrange(page => page.GetEnumerator()).Returns(() => { return secretItems.GetEnumerator(); });
            }
            else
            {
                var skipCount = nextLink < 0 ? 0 : nextLink;
                var thisCount = Math.Min((int)maxResults, secretItems.Count() - skipCount);

                if (thisCount >= secretItems.Count() - skipCount)
                {
                    pages.Arrange(page => page.NextPageLink).Returns(string.Empty);
                    pages.Arrange(page => page.GetEnumerator()).Returns(() => { return secretItems.Skip(skipCount).GetEnumerator(); });
                }
                else
                {
                    pages.Arrange(page => page.NextPageLink).Returns($"{maxResults}|{Convert.ToString(skipCount + maxResults)}");
                    pages.Arrange(page => page.GetEnumerator()).Returns(() => { return secretItems.Skip(skipCount).Take(thisCount).GetEnumerator(); });
                }
            }

            var result = new AzureOperationResponse<IPage<SecretItem>> { Body = pages, Response = new System.Net.Http.HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK } };
            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}