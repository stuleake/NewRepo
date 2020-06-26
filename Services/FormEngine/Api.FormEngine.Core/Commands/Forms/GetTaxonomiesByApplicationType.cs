using Api.FormEngine.Core.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Api.FormEngine.Core.Commands.Forms
{
    /// <summary>
    /// Class for taxonomies by application type
    /// </summary>
    public class GetTaxonomiesByApplicationType : BaseCommand, IRequest<List<QuestionSetWithTaxonomies>>
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