using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Collections.Generic;

namespace Api.FormEngine.Core.Services.Parsers.Models
{
    /// <summary>
    /// Class representing the required data used for parsing taxonomies.
    /// </summary>
    public class TaxonomyParserModel
    {
        /// <summary>
        /// Gets or sets the taxonomies.
        /// </summary>
        public IDictionary<string, string> Taxonomies { get; set; }

        /// <summary>
        /// Gets or sets the question sets associates with the taxonomies.
        /// </summary>
        public IEnumerable<QuestionSet> QuestionSets { get; set; }
    }
}