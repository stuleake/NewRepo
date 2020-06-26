using MediatR;
using System;
using TQ.Core.Filters;

namespace Api.FormEngine.Core.Commands.Forms
{
    /// <summary>
    /// A model to create question set
    /// </summary>
    public class CreateQuestionSetResponse : BaseCommand, IRequest<Guid>
    {
        /// <summary>
        /// Gets or Sets Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or Sets Response of operation
        /// </summary>
        [CustomInputValidate]
        public string Response { get; set; }

        /// <summary>
        /// Gets or Sets Question Set Id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or sets the application type id
        /// </summary>
        public Guid ApplicationTypeId { get; set; }
    }
}