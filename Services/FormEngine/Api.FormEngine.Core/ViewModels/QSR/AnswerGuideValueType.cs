using System;

namespace Api.FormEngine.Core.ViewModels.QSR
{
    /// <summary>
    /// AnswerGuide Value Type Model
    /// </summary>
    public class AnswerGuideValueType
    {
        /// <summary>
        /// Gets or Sets the Field Id
        /// </summary>
        public Guid Fieldid { get; set; }

        /// <summary>
        /// Gets or Sets the AnswerGuide Id
        /// </summary>
        public Guid AnswerGuideId { get; set; }

        /// <summary>
        /// Gets or Sets the Value
        /// </summary>
        public string Value { get; set; }
    }
}