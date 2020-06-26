using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model for RuleDef and Parameter mapping
    /// </summary>
    [Table("RuleDefParameterMappings", Schema = FeeCalculatorSchemas.Dbo)]
    public class RuleDefParameterMapping
    {
        /// <summary>
        /// Gets or Sets the RuleDefParameterMappingId
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RuleDefParameterMappingId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definition Id
        /// </summary>
        public Guid RuleDefId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definition
        /// </summary>
        [ForeignKey("RuleDefId")]
        public RuleDef RuleDef { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter Id
        /// </summary>
        public Guid ParameterId { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter
        /// </summary>
        [ForeignKey("ParameterId")]
        public Parameter Parameter { get; set; }

        /// <summary>
        /// Gets or Sets the Sequence of parameter for the Rule input
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter type
        /// </summary>
        public string ParameterType { get; set; }

        /// <summary>
        /// Gets or Sets the output operation to be done on output
        /// </summary>
        public string OutputOperation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output value is final fee figure
        /// </summary>
        public bool IsFinalOutput { get; set; }
    }
}