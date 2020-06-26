using Api.PP2.Core.ViewModels;
using MediatR;

namespace Api.PP2.Core.Commands.SubmittedApplications
{
    /// <summary>
    /// A model to request list of submitted application for a user.
    /// </summary>
    public class SubmittedApplicationsRequestModel : BaseCommand, IRequest<ApplicationListModel>
    {
        /// <summary>
        /// Gets or Sets the Role assigned to a User.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or Sets the Portal
        /// </summary>
        public string Portal { get; set; }
    }
}