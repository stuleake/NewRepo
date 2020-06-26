using System;

namespace Api.Planner.Core.Commands.Forms.QuestionSet
{
    /// <summary>
    /// Model for Field Error Message
    /// </summary>
    public class FieldErrorMessage
    {
        /// <summary>
        /// Gets or Sets field id
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}