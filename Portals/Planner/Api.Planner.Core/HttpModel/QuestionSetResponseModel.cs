using System;

namespace Api.Planner.Core.HttpModel
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

        /// <summary>
        /// Gets or Sets the UserID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
    }
}