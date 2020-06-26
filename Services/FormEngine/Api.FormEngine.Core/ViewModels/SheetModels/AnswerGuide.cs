namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// Aswer Guide sheet model
    /// </summary>
    public class AnswerGuide
    {
        /// <summary>
        /// Gets or Sets Field Number
        /// </summary>
        public string FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets Answer Number
        /// </summary>
        public string AnsNo { get; set; }

        /// <summary>
        /// Gets or Sets Manimum Range of number
        /// </summary>
        public string RangeMin { get; set; }

        /// <summary>
        /// Gets or Sets Maximum Range of number
        /// </summary>
        public string RangeMax { get; set; }

        /// <summary>
        /// Gets or Sets Minimum Range of Text
        /// </summary>
        public string LengthMin { get; set; }

        /// <summary>
        ///  Gets or Sets Maximum Range of Text
        /// </summary>
        public string LengthMax { get; set; }

        /// <summary>
        /// Gets or Sets the minimum date
        /// </summary>
        public string DateMin { get; set; }

        /// <summary>
        /// Gets or Sets the maximum date
        /// </summary>
        public string DateMax { get; set; }

        /// <summary>
        ///  Gets or Sets Regex Pattern for frontend
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        ///  Gets or Sets Regex Pattern for backend
        /// </summary>
        public string RegexBE { get; set; }

        /// <summary>
        ///  Gets or Sets Multiple of value
        /// </summary>
        public string Multiple { get; set; }

        /// <summary>
        /// Gets or Sets label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///  Gets or Sets Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets of error label
        /// </summary>
        public string ErrorLabel { get; set; }

        /// <summary>
        ///  Gets or Sets API path
        /// </summary>
        public string Api { get; set; }

        /// <summary>
        ///  Gets or Sets Default value
        /// </summary>
        public string IsDefault { get; set; }
    }
}