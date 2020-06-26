using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.PlanningPortal.Schemas.Dbo
{
    /// <summary>
    /// A model for User Application Details
    /// </summary>
    [Table("UserApplications", Schema = PlanningPortalSchemas.Dbo)]
    public class UserApplication
    {
        /// <summary>
        /// Gets or Sets User Application Id
        /// </summary>
        [Key]
        public Guid UserApplicationId { get; set; }

        /// <summary>
        /// Gets or Sets the User Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or Sets Planning form Id
        /// </summary>
        public Guid PlanningFormId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user application has started or not
        /// </summary>
        public bool Started { get; set; }

        /// <summary>
        /// Gets or Sets th Last saved details for form
        /// </summary>
        public DateTime LastSaved { get; set; }

        /// <summary>
        /// Gets or Sets the Status of application
        /// </summary>
        public string Status { get; set; }
    }
}