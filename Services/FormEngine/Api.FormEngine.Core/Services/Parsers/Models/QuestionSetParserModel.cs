using Api.FormEngine.Core.ViewModels.SheetModels;
using System;

namespace Api.FormEngine.Core.Services.Parsers.Models
{
    /// <summary>
    /// POCO representing the required data inside the question set parser.
    /// </summary>
    public class QuestionSetParserModel
    {
        /// <summary>
        /// Gets or sets the current question set.
        /// </summary>
        public QuestionSet QuestionSet { get; set; }

        /// <summary>
        /// Gets or sets the URL of the excel application.
        /// </summary>
        public string ExcelUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public Guid UserId { get; set; }
    }
}