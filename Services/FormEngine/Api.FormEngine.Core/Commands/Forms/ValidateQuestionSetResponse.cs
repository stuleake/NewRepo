using Api.FormEngine.Core.ViewModels;
using MediatR;
using System;

namespace Api.FormEngine.Core.Commands.Forms
{
    /// <summary>
    /// Class to Validate Question Set Response
    /// </summary>
    public class ValidateQuestionSetResponse : BaseCommand, IRequest<QuestionSetValidate>
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