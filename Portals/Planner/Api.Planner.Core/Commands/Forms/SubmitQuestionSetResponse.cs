using Api.Planner.Core.ViewModels;
using MediatR;
using System;

namespace Api.Planner.Core.Commands.Forms
{
    /// <summary>
    /// Model To Submit question set response
    /// </summary>
    public class SubmitQuestionSetResponse : BaseCommand, IRequest<QuestionSetSubmit>
    {
        /// <summary>
        /// Gets or Sets QS Collection Id
        /// </summary>
        public Guid QSCollectionId { get; set; }

        /// <summary>
        /// Gets or Sets Question set Id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets Application name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the application type id
        /// </summary>
        public Guid ApplicationTypeId { get; set; }
    }
}