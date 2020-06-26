using System.ComponentModel.DataAnnotations;

namespace Api.FormEngine.Core.ViewModels.Enum
{
    /// <summary>
    /// Excel Sheet enum
    /// </summary>
    public enum Sheets
    {
        /// <summary>
        /// Field sheet
        /// </summary>
        [Display(Name = "Fields")]
        Fields,

        /// <summary>
        /// Answer Guide Sheet
        /// </summary>
        [Display(Name = "AnswerGuide")]
        AnswerGuide,

        /// <summary>
        /// Dependency sheet
        /// </summary>
        [Display(Name = "Dependencies")]
        Dependencies,

        /// <summary>
        /// Aggregation Sheet
        /// </summary>
        [Display(Name = "Aggregations")]
        Aggregations
    }
}