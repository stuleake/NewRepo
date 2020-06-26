using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model class for QsRule Mappings
    /// </summary>
    [Table("QsRuleMapping", Schema = FeeCalculatorSchemas.Dbo)]
    public class QSRuleMapping
    {
        /// <summary>
        /// Gets or Sets the QSRuleMapping Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QSRuleMappingId { get; set; }

        /// <summary>
        /// Gets or Sets the question set number
        /// </summary>
        public int QSNo { get; set; }

        /// <summary>
        /// Gets or Sets the question set version
        /// </summary>
        public string QSVersion { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definition Id
        /// </summary>
        public Guid RuleDefId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definition
        /// </summary>
        [ForeignKey("RuleDefId")]
        public RuleDef RuleDef { get; set; }
    }
}