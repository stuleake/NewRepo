namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// Dependency of field sheet model
    /// </summary>
    public class Dependencies
    {
        /// <summary>
        /// Gets or Sets Field Number
        /// </summary>
        public string FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets the section number
        /// </summary>
        public string SectionNo { get; set; }

        /// <summary>
        /// Gets or Sets Field Display
        /// </summary>
        public string FieldDisplay { get; set; }

        /// <summary>
        /// Gets or Sets Depends on Answer
        /// </summary>
        public string DependsOnAns { get; set; }

        /// <summary>
        /// Gets or Sets Depends on Answer from question set
        /// </summary>
        public string DependsOnAnsfromQS { get; set; }

        /// <summary>
        /// Gets or Sets Depends on field count
        /// </summary>
        public string DependsCount { get; set; }

        /// <summary>
        /// Gets or Sets Decides Section
        /// </summary>
        public string DecidesSection { get; set; }
    }
}