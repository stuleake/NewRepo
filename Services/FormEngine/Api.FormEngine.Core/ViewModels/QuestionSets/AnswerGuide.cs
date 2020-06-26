using System;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    ///  Model for Answer Guide
    /// </summary>
    public class AnswerGuide
    {
        /// <summary>
        /// Gets or Sets Answer Guide Id
        /// </summary>
        public Guid AnswerGuideId { get; set; }

        /// <summary>
        /// Gets or Sets Field Id
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or Sets Answer Type
        /// </summary>
        public string AnswerType { get; set; }

        /// <summary>
        /// Gets or Sets the min
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// Gets or Sets the max
        /// </summary>
        public string Max { get; set; }

        /// <summary>
        /// Gets or Sets the Lable
        /// </summary>
        public string AnswerGuideLabel { get; set; }

        /// <summary>
        /// Gets or Sets the value
        /// </summary>
        public string AnswerGuideValue { get; set; }

        /// <summary>
        /// Gets or Sets the Answer Guide Pattern
        /// </summary>
        public string AnswerGuidePattern { get; set; }

        /// <summary>
        /// Gets or Sets the isDefault
        /// </summary>
        public string IsDefault { get; set; }

        /// <summary>
        /// Gets or Sets the Sequence
        /// </summary>
        public int AnswerSequence { get; set; }

        /// <summary>
        /// Gets or Sets AnswerGuide Error
        /// </summary>
        public string AnswerGuideError { get; set; }
    }
}