using MediatR;
using System;

namespace Api.FormEngine.Core.Commands.Forms.Temp
{
    /// <summary>
    /// Model To Submit question set response
    /// </summary>
    public class SubmitQuestionSetResponse : BaseCommand, IRequest<bool>
    {
        /// <summary>
        /// Gets or Sets Question set Id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets QS Collection Id
        /// </summary>
        public Guid QSCollectionId { get; set; }

        /// <summary>
        /// Gets or Sets Application name
        /// </summary>
        public string ApplicationName { get; set; }

        public Guid ApplicationTypeId { get; set; }
    }
}