using Api.Planner.Core.ViewModels;
using MediatR;
using System;

namespace Api.Planner.Core.Commands.ApplicationList
{
    /// <summary>
    /// A model to request application list for a User.
    /// </summary>
    public class ApplicationListRequestModel : IRequest<ApplicationListModel>
    {
        /// <summary>
        /// Gets or Sets the type of application needed
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets the User Id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Country of User.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets the Authentication token of a user.
        /// </summary>
        public string AuthToken { get; set; }
    }
}