using System;

namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// Question Set Data
    /// </summary>
    public class QuestionSet
    {
        /// <summary>
        /// Gets or Sets Question Set id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets Definition of question set
        /// </summary>
        public string Definition { get; set; }
    }
}