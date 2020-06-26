namespace Api.FeeCalculator.Core.ViewModels
{
    /// <summary>
    /// The rule question model
    /// </summary>
    public class ParameterModel
    {
        /// <summary>
        /// Gets or Sets the Rule Question Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Data type of Rule Question
        /// </summary>
        public string Datatype { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter type
        /// </summary>
        public string ParameterType { get; set; }

        /// <summary>
        /// Gets or Sets the Output operation to be performed on output
        /// </summary>
        public string OutputOperation { get; set; }
    }
}