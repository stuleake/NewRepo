using Api.FormEngine.Core.ViewModels.SheetModels;
using System;

namespace Api.FormEngine.Core.Services.Parsers.Models
{
    /// <summary>
    /// Data to be used when parsing section mappings from an excel spread sheet.
    /// </summary>
    public class SectionMappingParserModel
    {
        /// <summary>
        /// Gets or sets the current question set.
        /// </summary>
        public QuestionSet QuestionSet { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
