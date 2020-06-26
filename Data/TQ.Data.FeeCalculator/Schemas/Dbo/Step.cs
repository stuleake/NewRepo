using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model class for Fee calculation steps
    /// </summary>
    [Table("Steps", Schema = FeeCalculatorSchemas.Dbo)]
    public class Step
    {
        /// <summary>
        /// Gets or Sets the Fee calculation Step Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StepId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Id
        /// </summary>
        public Guid RuleId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule
        /// </summary>
        [ForeignKey("RuleId")]
        public RuleDef RuleDef { get; set; }

        /// <summary>
        /// Gets or Sets the Session type
        /// </summary>
        public string SessionType { get; set; }

        /// <summary>
        /// Gets or Sets the Session type
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Gets or Sets the answer description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the category Id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets the category
        /// </summary>
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or Sets the reference id of Application
        /// </summary>
        public int ApplicationTypeRefId { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether rule output is final or not
        /// </summary>
        public bool IsFinalOutput { get; set; }
    }
}