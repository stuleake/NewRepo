using MediatR;

namespace Api.Globals.Core.Commands.SignUp
{
    /// <summary>
    /// Delete User Request Command
    /// </summary>
    public class DeleteUserRequest : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the email for the user
        /// </summary>
        public string Emailid { get; set; }
    }
}