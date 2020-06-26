using MediatR;

namespace Api.Admin.Core.Commands.AzureUser
{
    /// <summary>
    /// Azure User creation model
    /// </summary>
    public class AzureUserModel : IRequest<bool>
    {
        /// <summary>
        /// Gets or Sets Title of User
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets First Name of User
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets Last Name of User
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets Email address of User
        /// </summary>
        public string Email { get; set; }
    }
}