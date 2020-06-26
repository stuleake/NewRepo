using MediatR;
using System;

namespace Api.Planner.Core.Commands.Forms.QuestionSet
{
    /// <summary>
    /// Model to Get Question Set
    /// </summary>
    public class GetQuestionSet : BaseCommand, IRequest<ViewModels.QuestionSet>
    {
        /// <summary>
        /// Gets or Sets Id of Question Set
        /// </summary>
        public Guid Id { get; set; }
    }
}