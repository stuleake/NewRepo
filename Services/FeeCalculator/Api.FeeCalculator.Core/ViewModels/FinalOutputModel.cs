namespace Api.FeeCalculator.Core.ViewModels
{
    /// <summary>
    /// A model class for final output details for fee calculation
    /// </summary>
    public class FinalOutputModel
    {
        /// <summary>
        /// Gets or Sets the Parameter name
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or Sets the Data type
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or Sets the output value
        /// </summary>
        public string Value { get; set; }
    }
}