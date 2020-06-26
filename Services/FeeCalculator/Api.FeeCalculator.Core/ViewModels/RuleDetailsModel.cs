using System;

namespace Api.FeeCalculator.Core.ViewModels
{
    /// <summary>
    /// A model class containing Rule Details
    /// </summary>
    public class RuleDetailsModel
    {
        /// <summary>
        /// Gets or Sets the Rule Definition id
        /// </summary>
        public Guid RuleDefId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Name
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definition
        /// </summary>
        public string RuleDef { get; set; }

        /// <summary>
        /// Gets or Sets the Rule reference Id
        /// </summary>
        public int RuleReferenceId { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter Id
        /// </summary>
        public Guid ParameterId { get; set; }

        /// <summary>
        /// Gets or Sets the parameter name
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or Sets the Region Id
        /// </summary>
        public Guid RegionId { get; set; }

        /// <summary>
        /// Gets or Sets the Data type id
        /// </summary>
        public Guid DataTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the datatype
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or Sets the sequence of rule parameter
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or Sets the Category Id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets the parameter type
        /// </summary>
        public string ParamType { get; set; }

        /// <summary>
        /// Gets or Sets the output operation
        /// </summary>
        public string OutputOperation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output is final or not
        /// </summary>
        public bool IsFinalOutput { get; set; }
    }
}