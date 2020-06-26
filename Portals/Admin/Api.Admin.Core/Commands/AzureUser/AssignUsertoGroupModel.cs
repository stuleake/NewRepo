using MediatR;

namespace Api.Admin.Core.Commands.AzureUser
{
    /// <summary>
    /// Azure assign User to Groups model
    /// </summary>
    public class AssignUsertoGroupModel : IRequest<bool>
    {
        /// <summary>
        /// Gets or Sets the User Object Id.
        /// </summary>
        public string UserObjectId { get; set; }

        /// <summary>
        /// Gets or Sets the Group Object Id.
        /// </summary>
        public string GroupObjectId { get; set; }
    }
}