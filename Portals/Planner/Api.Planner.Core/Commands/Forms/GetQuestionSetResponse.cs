using MediatR;
using System;

namespace Api.Planner.Core.Commands.Forms
{
    /// <summary>
    /// Model to Get Question Set Response
    /// </summary>
    public class GetQuestionSetResponse : BaseCommand, IRequest<ViewModels.QuestionSetResponse>
    {
        /// <summary>
        /// Gets or Sets QS Collection Id
        /// </summary>
        public Guid QSCollectionId { get; set; }

        /// <summary>
        /// Gets or Sets Question Set id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets Application Name
        /// </summary>
        public string ApplicationName { get; set; }
    }
}