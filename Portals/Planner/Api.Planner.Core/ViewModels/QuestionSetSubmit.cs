using Api.Planner.Core.Commands.Forms.QuestionSet;
using System;
using System.Collections.Generic;

namespace Api.Planner.Core.ViewModels
{
    /// <summary>
    /// A model for QuestionSet Submit
    /// </summary>
    public class QuestionSetSubmit
    {
        /// <summary>
        /// Gets or sets a value indicating whether Status is true or false
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or Sets Erros
        /// </summary>
        public IEnumerable<string> Errors { get; set; }

        /// <summary>
        /// Gets or Sets Response
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or Sets QuestionSet Id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets UserApplication Id
        /// </summary>
        public Guid UserApplicationId { get; set; }

        /// <summary>
        /// Gets or Sets QuestionSetResponseId
        /// </summary>
        public Guid QuestionSetResponseId { get; set; }

        /// <summary>
        /// Gets or Sets the ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or Sets the Field Error Message
        /// </summary>
        public IList<FieldErrorMessage> ErrorMessage { get; set; }
    }
}