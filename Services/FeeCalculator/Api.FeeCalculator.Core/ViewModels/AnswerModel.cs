namespace Api.FeeCalculator.Core.ViewModels
{
    /// <summary>
    /// A model class for Fee Calculator input parameters Answers
    /// </summary>
    public class AnswerModel
    {
        /// <summary>
        /// Gets or Sets the parameter name
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or Sets the datatype
        /// </summary>
        public string Datatype { get; set; }

        /// <summary>
        /// Gets or Sets the Answer
        /// </summary>
        public string Answer { get; set; }
    }
}