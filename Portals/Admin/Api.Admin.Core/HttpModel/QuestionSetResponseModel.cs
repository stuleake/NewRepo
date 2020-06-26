using System;

namespace Api.Admin.Core.HttpModel
{
    /// <summary>
    /// A model class for Question Set Response
    /// </summary>
    public class QuestionSetResponseModel
    {
        /// <summary>
        /// Gets or Sets the response
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or Sets the Question Id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets ApplicationId
        /// </summary>
        public Guid ApplicationId { get; set; }
    }
}