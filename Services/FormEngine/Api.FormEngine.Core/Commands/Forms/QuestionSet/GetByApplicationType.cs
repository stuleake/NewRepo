using MediatR;
using System;
using System.Collections.Generic;

namespace Api.FormEngine.Core.Commands.Forms.QuestionSet
{
    /// <summary>
    /// Command to get question sets by application type
    /// </summary>
    public class GetByApplicationType : BaseCommand, IRequest<List<ViewModels.QuestionSet>>
    {
        /// <summary>
        /// Gets or sets the application type
        /// </summary>
        public Guid QSCollectionTypeId { get; set; }
    }
}