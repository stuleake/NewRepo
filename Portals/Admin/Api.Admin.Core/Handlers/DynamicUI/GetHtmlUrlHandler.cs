using Api.Admin.Core.Commands.DynamicUI;
using Api.Admin.Core.ViewModels;
using CT.KeyVault;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;

namespace Api.Admin.Core.Handlers.DynamicUI
{
    /// <summary>
    /// Handler to get HTML URL for dynamic UI
    /// </summary>
    public class GetHtmlUrlHandler : IRequestHandler<GetHtmlUrl, string>
    {
        private readonly IVaultManager vaultManager;
        private readonly IConfiguration configuration;
        private readonly IDynamicHtmlPage dynamicHtmlPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHtmlUrlHandler"/> class.
        /// </summary>
        /// <param name="vaultManager">object of VaultManager to access secret</param>
        /// <param name="configuration">configuration data</param>
        /// <param name="dynamicHtmlPage">the dynamicHtmlPage to use</param>
        public GetHtmlUrlHandler(IVaultManager vaultManager, IConfiguration configuration, IDynamicHtmlPage dynamicHtmlPage)
        {
            this.vaultManager = vaultManager;
            this.configuration = configuration;
            this.dynamicHtmlPage = dynamicHtmlPage;
        }

        /// <inheritdoc/>
        public async Task<string> Handle(GetHtmlUrl request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // default HTML
            if (string.IsNullOrWhiteSpace(request.Country) || (request?.Country.ToLower() != CountryConstants.England.ToLower() && request?.Country.ToLower() != CountryConstants.Wales.ToLower()))
            {
                request.Country = CountryConstants.England.ToLower();
            }

            var countryUrl = await vaultManager.GetSecretAsync(configuration["DynamicUI:" + request?.Country + "_HtmlPage"]).ConfigureAwait(false);
            string data = await this.dynamicHtmlPage.GetDymanicHtmlPageAsync(countryUrl).ConfigureAwait(false);

            return data;
        }
    }
}