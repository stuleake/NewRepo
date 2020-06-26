using Api.Globals.Core.Commands.ActivateUser;
using Api.Globals.Core.ViewModels;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Core.Helpers;

namespace Api.Globals.Core.Handlers.ActivateUser
{
    /// <summary>
    /// Handler to activate user
    /// </summary>
    public class ActivateUserHandler : IRequestHandler<ActivateUserRequest, bool>
    {
        private readonly B2CGraphClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivateUserHandler"/> class.
        /// </summary>
        /// <param name="client">The B2C client</param>
        public ActivateUserHandler(B2CGraphClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(ActivateUserRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var existingUser = await client.GetUserByEmailAsync(request.EmailId).ConfigureAwait(false);
            var existingUserObj = JsonConvert.DeserializeObject<AzureUserDataViewModel>(existingUser);
            if (existingUserObj.Value.Any())
            {
                if (!existingUserObj.Value.First().AccountEnabled)
                {
                    // Activate User
                    AzureUpdateInactiveUserViewModel updateModel = new AzureUpdateInactiveUserViewModel { AccountEnabled = true };
                    string azureUser = JsonConvert.SerializeObject(updateModel);
                    await client.UpdateUserAsync(existingUserObj.Value.First().ObjectId, azureUser).ConfigureAwait(false);
                }
                else
                {
                    throw new TQException("error_activeuserexist");
                }
            }
            return true;
        }
    }
}