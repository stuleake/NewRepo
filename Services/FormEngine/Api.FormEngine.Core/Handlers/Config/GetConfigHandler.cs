using Api.FormEngine.Core.Commands.Config;
using CT.KeyVault;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.FormEngine.Core.Handlers.Config
{
    /// <summary>
    /// Handler for Azure secrets configurations
    /// </summary>
    public class GetConfigHandler : IRequestHandler<GetConfig, ViewModels.Config>
    {
        private readonly IVaultManager vaultManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfigHandler"/> class.
        /// </summary>
        /// <param name="vaultManager">VaultManager object to access secrets</param>
        public GetConfigHandler(IVaultManager vaultManager)
        {
            this.vaultManager = vaultManager;
        }

        /// <inheritdoc/>
        public async Task<ViewModels.Config> Handle(GetConfig request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ViewModels.Config result = new ViewModels.Config
            {
                Secrets = new Dictionary<string, string>()
            };

            // Get the entries from key value one by one.
            foreach (var key in request.Keys)
            {
                var secret = await vaultManager.GetSecretAsync(key).ConfigureAwait(false);
                result.Secrets.Add(key.Replace("-", "_", StringComparison.InvariantCulture), secret);
            }

            return result;
        }
    }
}