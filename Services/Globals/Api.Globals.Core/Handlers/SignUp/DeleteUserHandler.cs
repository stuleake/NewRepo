using Api.Globals.Core.Commands.SignUp;
using Api.Globals.Core.ViewModels;
using MediatR;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Core.Helpers;

namespace Api.Globals.Core.Handlers.SignUp
{
    /// <summary>
    /// Handler to delete user
    /// </summary>
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, bool>
    {
        private readonly B2CGraphClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserHandler"/> class.
        /// </summary>
        /// <param name="client">The B2C graph client</param>
        public DeleteUserHandler(B2CGraphClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new System.ArgumentNullException(nameof(request));
            }

            var existingUser = await client.GetUserByEmailAsync(request.Emailid).ConfigureAwait(false);
            var existingUserObj = JsonConvert.DeserializeObject<AzureUserDataViewModel>(existingUser);
            if (existingUserObj.Value.Any())
            {
                await client.DeleteUserAsync(existingUserObj.Value.First().ObjectId).ConfigureAwait(false);
            }
            else
            {
                throw new TQException("user_doesnot_exists");
            }

            return true;
        }
    }
}