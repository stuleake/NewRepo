using Api.Admin.Core.Commands.AzureUser;
using Api.Admin.Core.ViewModels;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Helpers;

namespace Api.Admin.Core.Handlers.AzureUser
{
    /// <summary>
    /// Get Azure User from B2C
    /// </summary>
    public class GetAzureUserHandler : IRequestHandler<AzureUserHtmlUrl, IEnumerable<AzureUserObject>>
    {
        private readonly IB2CGraphClient client;

        /// <summary>
        ///  Initializes a new instance of the <see cref="GetAzureUserHandler"/> class.
        /// </summary>
        /// <param name="client">B2Cgraphclient</param>
        public GetAzureUserHandler(IB2CGraphClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AzureUserObject>> Handle(AzureUserHtmlUrl request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var userList = await this.client.GetAllUsersAsync(request.Active).ConfigureAwait(false);
            var azureUserdetails = JsonConvert.DeserializeObject<AzureUserDataViewModel>(userList);

            return azureUserdetails.Value;
        }
    }
}