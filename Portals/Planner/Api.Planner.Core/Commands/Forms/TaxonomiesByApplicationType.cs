using Api.Planner.Core.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Api.Planner.Core.Commands.Forms
{
    /// <summary>
    /// Class for taxonomies by application type
    /// </summary>
    public class TaxonomiesByApplicationType : BaseCommand, IRequest<List<QuestionSetWithTaxonomies>>
    {
        /// <summary>
        /// Gets or sets ApplicationTypeRefNo
        /// </summary>
        public int ApplicationTypeRefNo { get; set; }

        /// <summary>
        /// Gets or sets Language
        /// </summary>
        public string Language { get; set; }
    }
}