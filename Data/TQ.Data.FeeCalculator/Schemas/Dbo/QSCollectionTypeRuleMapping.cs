using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model class for QS collection type rule mappings
    /// </summary>
    [Table("QSCollectionTypeRuleMappings", Schema = FeeCalculatorSchemas.Dbo)]
    public class QSCollectionTypeRuleMapping
    {
        /// <summary>
        /// Gets or Sets the QS Collection type Rule Mapping Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QSCollectionTypeRuleMappingId { get; set; }

        /// <summary>
        /// Gets or Sets the Application Type Reference no
        /// </summary>
        public int ApplicationTypeRefNo { get; set; }

        /// <summary>
        /// Gets or Sets the Qs Collection Version
        /// </summary>
        public string QSCollectionVersion { get; set; }

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