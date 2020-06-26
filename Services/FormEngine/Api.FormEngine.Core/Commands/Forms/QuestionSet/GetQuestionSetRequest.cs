using Api.FormEngine.Core.ViewModels.QuestionSets;
using MediatR;
using System;

namespace Api.FormEngine.Core.Commands.Forms.QuestionSet
{
    /// <summary>
    /// Class to Get Question Set
    /// </summary>
    public class GetQuestionSetRequest : IRequest<QuestionSetDetails>
    {
        /// <summary>
        /// Gets or Sets Id of Question Set
        /// </summary>
        public Guid Id { get; set; }
    }
}