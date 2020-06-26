using System;

namespace Api.FeeCalculator.Core.ViewModels
{
    /// <summary>
    /// A model class containing calculation steps for rule execution
    /// </summary>
    public class CalculationStepModel
    {
        /// <summary>
        /// Gets or Sets the Rule Id
        /// </summary>
        public Guid RuleId { get; set; }

        /// <summary>
        /// Gets or Sets the answer description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the Rule no
        /// </summary>
        public int RuleNo { get; set; }

        /// <summary>
        /// Gets or Sets the input for rule
        /// </summary>
        public string Inputs { get; set; }

        /// <summary>
        /// Gets or Sets the output of rule execution
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Gets or Sets the output parameter name
        /// </summary>
        public string OutputParamName { get; set; }

        /// <summary>
        /// Gets or Sets the output data type
        /// </summary>
        public string OutputDataType { get; set; }
    }
}