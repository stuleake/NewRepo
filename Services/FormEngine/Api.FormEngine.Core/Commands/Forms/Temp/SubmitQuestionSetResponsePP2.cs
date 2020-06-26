using MediatR;
using System;

namespace Api.FormEngine.Core.Commands.Forms.Temp
{
    /// <summary>
    /// Model To Submit question set response
    /// </summary>
    public class SubmitQuestionSetResponsePP2 : BaseCommand, IRequest<Guid>
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

        public string ApplicationName { get; set; }
    }
}