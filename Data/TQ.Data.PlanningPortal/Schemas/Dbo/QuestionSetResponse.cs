using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.PlanningPortal.Schemas.Dbo
{
    /// <summary>
    /// A Model to  manage QuestionSetResponse
    /// </summary>
    [Table("QuestionSetResponses", Schema = PlanningPortalSchemas.Dbo)]
    public class QuestionSetResponse
    {
        /// <summary>
        /// Gets or Sets QuestionSet Response Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuestionSetResponseId { get; set; }

        /// <summary>
        /// Gets or Sets User Application Id
        /// </summary>
        public Guid UserApplicationId { get; set; }

        /// <summary>
        /// Gets or Sets Response Message
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or Sets Question set Id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets Last saved details
        /// </summary>
        public DateTime LastSaved { get; set; }

        /// <summary>
        /// Gets or Sets Modify by details
        /// </summary>
        public Guid ModifiedBy { get; set; }
    }
}