using Api.Admin.Core.Commands.AzureUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Helpers;

namespace Api.Admin.Core.Handlers.AzureUser
{
    /// <summary>
    /// Get Azure User Group Handler
    /// </summary>
    public class GetAzureUserGroupHandler : IRequestHandler<AzureUserGroupHtmlUrl, IEnumerable<string>>
    {
        private readonly IB2CGraphClient client;

        /// <summary>
        ///  Initializes a new instance of the <see cref="GetAzureUserGroupHandler"/> class.
        /// </summary>
        /// <param name="client">B2Cgraphclient</param>
        public GetAzureUserGroupHandler(IB2CGraphClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> Handle(AzureUserGroupHtmlUrl request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var userGroups = await this.client.GetUserGroupByObjectIdAsync(request.ObjectId).ConfigureAwait(false);

            return userGroups;
        }
    }
}