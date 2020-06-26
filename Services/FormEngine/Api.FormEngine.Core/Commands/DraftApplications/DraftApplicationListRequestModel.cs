using Api.FormEngine.Core.ViewModels;
using MediatR;
using System;

namespace Api.FormEngine.Core.Commands.DraftApplications
{
    /// <summary>
    /// A model class to request all the draft applications for a User
    /// </summary>
    public class DraftApplicationListRequestModel : IRequest<ApplicationListModel>
    {
        /// <summary>
        /// Gets or Sets the Id of a user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Role assigned to a User.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or Sets the Country of User.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets the Portal
        // </summary>
        public string Portal { get; set; }
    }
}