using System;

namespace Api.FeeCalculator.Core.ViewModels
{
    /// <summary>
    /// A model to contain user application data.
    /// </summary>
    public class UserApplication
    {
        /// <summary>
        /// Gets or Sets Id of User Application.
        /// </summary>
        public Guid UserApplicationId { get; set; }

        /// <summary>
        /// Gets or Sets Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or Sets User Id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets the form Id.
        /// </summary>
        public Guid PlanningFormId { get; set; }

        /// <summary>
        /// Gets or Sets the status of user application.
        /// </summary>
        public string Status { get; set; }
    }
}