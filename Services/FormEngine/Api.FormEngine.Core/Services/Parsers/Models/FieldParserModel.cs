using Api.FormEngine.Core.ViewModels.SheetModels;
using System;
using System.Collections.Generic;

namespace Api.FormEngine.Core.Services.Parsers.Models
{
    /// <summary>
    /// Data to be used when parsing fields from an excel data sheet.
    /// </summary>
    public class FieldParserModel
    {
        /// <summary>
        /// Gets or sets the fields to parse.
        /// </summary>
        public IEnumerable<Field> Fields { get; set; }

        /// <summary>
        /// Gets or sets the dependencies used when parsing.
        /// </summary>
        public IEnumerable<Dependencies> Dependencies { get; set; }

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