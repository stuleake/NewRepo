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
    /// Handler to get the reset password HTML URL
    /// </summary>
    public class GetResetPasswordHtmlUrlHandler : IRequestHandler<GetResetPasswordHtmlUrl, string>
    {
        private readonly IVaultManager vaultManager;
        private readonly IConfiguration configuration;
        private readonly IDynamicHtmlPage dynamicHtmlPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetResetPasswordHtmlUrlHandler"/> class.
        /// </summary>
        /// <param name="vaultManager">object of VaultManager to access secret</param>
        /// <param name="configuration">configuration data</param>
        /// <param name="dynamicHtmlPage">the dynamicHtmlPage to use</param>
        public GetResetPasswordHtmlUrlHandler(IVaultManager vaultManager, IConfiguration configuration, IDynamicHtmlPage dynamicHtmlPage)
        {
            this.vaultManager = vaultManager;
            this.configuration = configuration;
            this.dynamicHtmlPage = dynamicHtmlPage;
        }

        /// <inheritdoc/>
        public async Task<string> Handle(GetResetPasswordHtmlUrl request, CancellationToken cancellationToken)
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

            var countryPageUrl = await vaultManager.GetSecretAsync(configuration["DynamicUI:" + request?.Country.ToLower() + "_ResetPasswordHtmlPage"]).ConfigureAwait(false);
            string data = await this.dynamicHtmlPage.GetDymanicHtmlPageAsync(countryPageUrl).ConfigureAwait(false);

            return data;
        }
    }
}