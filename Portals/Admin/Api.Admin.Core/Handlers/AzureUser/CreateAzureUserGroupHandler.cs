using Api.Admin.Core.Commands.AzureUser;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Helpers;

namespace Api.Admin.Core.Handlers.AzureUser
{
    /// <summary>
    /// Create Azure Group Handler
    /// </summary>
    public class CreateAzureUserGroupHandler : IRequestHandler<AzureUserGroupModel, bool>
    {
        private readonly IB2CGraphClient client;

        /// <summary>
        ///  Initializes a new instance of the <see cref="CreateAzureUserGroupHandler"/> class.
        /// </summary>
        /// <param name="client">B2Cgraphclient</param>
        public CreateAzureUserGroupHandler(IB2CGraphClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(AzureUserGroupModel request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException($"{nameof(request)}");
            }

            string azureGroup = JsonConvert.SerializeObject(request);
            var isGroups = await this.client.CreateGroupAsync(azureGroup).ConfigureAwait(false);

            return isGroups;
        }
    }
}