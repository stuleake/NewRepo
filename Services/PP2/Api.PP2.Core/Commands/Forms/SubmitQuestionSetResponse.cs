using MediatR;
using System;

namespace Api.PP2.Core.Commands.Forms
{
    /// <summary>
    /// Model To Submit question set response
    /// </summary>
    public class SubmitQuestionSetResponse : BaseCommand, IRequest<Guid>
    {
        /// <summary>
        /// Gets or Sets Response
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or Sets Question set Id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets Application Id
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Gets or Sets the Application Name
        /// </summary>
        public string ApplicationName { get; set; }
    }
}