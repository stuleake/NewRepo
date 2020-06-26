using System.ComponentModel.DataAnnotations;

namespace TQ.Core.Enums
{
    /// <summary>
    /// enum for AnswerTypes
    /// </summary>
    public enum AnswerTypes
    {
        /// <summary>
        /// Range
        /// </summary>
        [Display(Name = "Range")]
        Range = 1,

        /// <summary>
        /// Length
        /// </summary>
        [Display(Name = "Length")]
        Length = 2,

        /// <summary>
        /// Regex UI
        /// </summary>
        [Display(Name = "Regex")]
        Regex = 3,

        /// <summary>
        /// Regex BE
        /// </summary>
        [Display(Name = "RegexBE")]
        RegexBE = 4,

        /// <summary>
        /// Multiple
        /// </summary>
        [Display(Name = "Multiple")]
        Multiple = 5,

        /// <summary>
        /// Value
        /// </summary>
        [Display(Name = "Value")]
        Value = 6,

        /// <summary>
        /// API
        /// </summary>
        [Display(Name = "API")]
        API = 7,

        /// <summary>
        /// Date
        /// </summary>
        [Display(Name = "Date")]
        Date = 8,

        /// <summary>
        /// CopyFrom
        /// </summary>
        [Display(Name = "CopyFrom")]
        CopyFrom = 9,
    }
}