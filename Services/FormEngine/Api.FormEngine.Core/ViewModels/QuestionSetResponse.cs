using System;

namespace Api.FormEngine.Core.ViewModels
{
    /// <summary>
    /// A model for Question set response
    /// </summary>
    public class QuestionSetResponse
    {
        /// <summary>
        /// Gets or Sets QuestionSetResponseId
        /// </summary>
        public Guid QuestionSetResponseId { get; set; }

        /// <summary>
        /// Gets or Sets UserApplicationeId
        /// </summary>
        public Guid UserApplicationId { get; set; }

        /// <summary>
        /// Gets or Sets Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or Sets Response
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or Sets QuestionSetId
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets LastSaved
        /// </summary>
        public DateTime LastSaved { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        public Guid ModifiedBy { get; set; }
    }
}