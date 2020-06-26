using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A database model for Rule Definition
    /// </summary>
    [Table("RuleDefs", Schema = FeeCalculatorSchemas.Dbo)]
    public class RuleDef
    {
        /// <summary>
        /// Gets or Sets the Rule Definition Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RuleDefId { get; set; }

        /// <summary>
        /// Gets or Sets the Fee CalculatorDef Package Id
        /// </summary>
        public Guid DefPackageId { get; set; }

        /// <summary>
        /// Gets or Sets the fee calculator definition package data
        /// </summary>
        [ForeignKey("DefPackageId")]
        public virtual DefPackage DefPackage { get; set; }

        /// <summary>
        /// Gets or Sets the Reference Id of Rule
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Name
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definition
        /// </summary>
        public string RuleDefinition { get; set; }

        /// <summary>
        /// Gets or Sets the Category Id
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets the category
        /// </summary>
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or Sets the Tenant
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or Sets the product
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or Sets the start date of the rule
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or Sets the End date of rule application
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or Sets the Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets the User Id that created Rule
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets the list of RuleDef and parameter mapping
        /// </summary>
        public IList<RuleDefParameterMapping> RuleDefParameterMappings { get; set; }
    }
}