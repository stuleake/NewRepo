using Api.Admin.Core.Commands.AzureUser;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Helpers;

namespace Api.Admin.Core.Handlers.AzureUser
{
    /// <summary>
    /// Assign user to group
    /// </summary>
    public class AzureAssignUsertoGroupHandler : IRequestHandler<AssignUsertoGroupModel, bool>
    {
        private readonly IB2CGraphClient client;

        /// <summary>
        ///  Initializes a new instance of the <see cref="AzureAssignUsertoGroupHandler"/> class.
        /// </summary>
        /// <param name="client">B2Cgraphclient</param>
        public AzureAssignUsertoGroupHandler(IB2CGraphClient client)
        {
            this.client = client;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(AssignUsertoGroupModel request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var isGroups = await this.client.AssignUserToGroupAsync(request.GroupObjectId, request.UserObjectId).ConfigureAwait(false);

            return isGroups;
        }
    }
}