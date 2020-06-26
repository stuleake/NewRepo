using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// List of sheet model
    /// </summary>
    public class ExcelSheetsData
    {
        /// <summary>
        /// Gets or Sets list of Question Set
        /// </summary>
        public IList<QuestionSet> QuestionSet { get; set; }

        /// <summary>
        /// Gets or Sets list of Answer Guide
        /// </summary>
        public IList<AnswerGuide> AnswerGuides { get; set; }

        /// <summary>
        /// Gets or Sets list of Dependencies
        /// </summary>
        public IList<Dependencies> Dependencies { get; set; }

        /// <summary>
        /// Gets or Sets list of Aggregations
        /// </summary>
        public IList<Aggregations> Aggregations { get; set; }
    }
}