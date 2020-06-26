using Api.Globals.Core.Commands.LastLogin;
using Api.Globals.Core.ViewModels;
using CT.KeyVault;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Helpers;

namespace Api.Globals.Core.Handlers.LastLogin
{
    /// <summary>
    /// The Last Login Handler class
    /// </summary>
    public class LastLoginHandler : IRequestHandler<LastLoginRequest, bool>
    {
        private readonly B2CGraphClient client;
        private readonly IVaultManager vaultManager;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastLoginHandler"/> class.
        /// </summary>
        /// <param name="client">The B2C client</param>
        /// <param name="vaultManager">The vault manager</param>
        /// <param name="configuration">The configuration</param>
        public LastLoginHandler(B2CGraphClient client, IVaultManager vaultManager, IConfiguration configuration)
        {
            this.client = client;
            this.vaultManager = vaultManager;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(LastLoginRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var existingUser = await client.GetUserByEmailAsync(request.EmailId).ConfigureAwait(false);
            var existingUserObj = JsonConvert.DeserializeObject<AzureUserDataViewModel>(existingUser);
            if (existingUserObj.Value.Any())
            {
                string azureUser = string.Format("{{\"{0}\":\"{1}\"}}", vaultManager.GetSecret(configuration["ExtensionLastLogin"]), DateTime.Now.ToString());
                await client.UpdateUserAsync(existingUserObj.Value.First().ObjectId, azureUser).ConfigureAwait(false);
            }
            return true;
        }
    }
}