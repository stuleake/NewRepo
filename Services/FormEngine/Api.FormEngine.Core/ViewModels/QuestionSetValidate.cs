using Api.FormEngine.Core.ViewModels.QuestionSets;
using System;
using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels
{
    /// <summary>
    /// A model for Question set validate
    /// </summary>
    public class QuestionSetValidate
    {
        /// <summary>
        /// Gets or sets Question set response Id
        /// </summary>
        public Guid QuestionSetResponseId { get; set; }

        /// <summary>
        /// Gets or sets List of Errors
        /// </summary>
        public IEnumerable<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets Question set Id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or sets User Application Id
        /// </summary>
        public Guid UserApplicationId { get; set; }

        /// <summary>
        /// Gets or Sets the Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets Response
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Status is true or false
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or Sets the Field Error Message
        /// </summary>
        public IList<FieldErrorMessage> ErrorMessage { get; set; }
    }
}