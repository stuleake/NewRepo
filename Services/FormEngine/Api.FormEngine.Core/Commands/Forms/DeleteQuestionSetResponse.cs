using MediatR;
using System;

namespace Api.FormEngine.Core.Commands.Forms
{
    /// <summary>
    /// Class to Delete Question Set Response
    /// </summary>
    public class DeleteQuestionSetResponse : IRequest<bool>
    {
        /// <summary>
        /// Gets or Sets Id of Question Set
        /// </summary>
        public Guid Id { get; set; }
    }
}