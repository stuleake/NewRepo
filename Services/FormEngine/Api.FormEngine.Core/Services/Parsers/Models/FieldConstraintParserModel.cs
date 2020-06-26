using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Collections.Generic;

namespace Api.FormEngine.Core.Services.Parsers.Models
{
    /// <summary>
    /// Model containing relevant information for parsing field constraints.
    /// </summary>
    public class FieldConstraintParserModel
    {
        /// <summary>
        /// Gets or sets the field constraint's dependencies.
        /// </summary>
        public IEnumerable<Dependencies> Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the field constraint's question set.
        /// </summary>
        public QuestionSet QuestionSet { get; set; }
    }
}