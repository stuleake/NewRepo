using MediatR;
using System;
using System.Collections.Generic;

namespace Api.Planner.Core.Commands.Forms.QuestionSet
{
    /// <summary>
    /// class to manage Application Type
    /// </summary>
    public class GetByApplicationType : BaseCommand, IRequest<List<ViewModels.QuestionSet>>
    {
        /// <summary>
        /// Gets or sets the application type
        /// </summary>
        public Guid QSCollectionTypeId { get; set; }
    }
}