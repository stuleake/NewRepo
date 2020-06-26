using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.Session.Model
{
    /// <summary>
    /// A Model for Question Set response
    /// </summary>
    public class QuestionSetResponse
    {
        /// <summary>
        /// Gets or Sets Question Set response ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuestionSetResponseId { get; set; }

        /// <summary>
        /// Gets or Sets the User Application Id
        /// </summary>
        public Guid UserApplicationId { get; set; }

        /// <summary>
        /// Gets or Sets User application
        /// </summary>
        [ForeignKey("UserApplicationId")]
        public UserApplication UserApplication { get; set; }

        /// <summary>
        /// Gets or Sets Response Message
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or Sets Question set Id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets Last Save Details
        /// </summary>
        public DateTime LastSaved { get; set; }

        /// <summary>
        /// Gets or Sets the Modification Details
        /// </summary>
        public Guid ModifiedBy { get; set; }
    }
}