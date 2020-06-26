using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Field
    /// </summary>

    [Table("Fields", Schema = FormEngineSchemas.Forms)]
    public class Field
    {
        /// <summary>
        /// Gets or Sets Field Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or Sets the FieldNo
        /// </summary>
        public int FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets the FieldVersion
        /// </summary>
        [Column(TypeName = "decimal(18,1)")]
        public decimal FieldVersion { get; set; }

        /// <summary>
        /// Gets or Sets the Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets the Helptext
        /// </summary>
        public string Helptext { get; set; }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Field Type
        /// </summary>
        public int? FieldTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the field Type definition data
        /// </summary>
        [ForeignKey("FieldTypeId")]
        public virtual FieldType FieldType { get; set; }

        /// <summary>
        /// Gets or Sets Field Display Id
        /// </summary>
        public int? DisplayId { get; set; }

        /// <summary>
        /// Gets or Sets the field Type definition data
        /// </summary>
        [ForeignKey("DisplayId")]
        public virtual Display Display { get; set; }

        /// <summary>
        /// Gets or Sets Display Constraint Id
        /// </summary>
        public int? DisplayConstraintId { get; set; }

        /// <summary>
        /// Gets or Sets the Constraint definition data
        /// </summary>
        [ForeignKey("DisplayConstraintId")]
        public virtual Constraint Constraint { get; set; }

        /// <summary>
        /// Gets or Sets  Constraint RuleId
        /// </summary>
        public int? ConstraintRuleId { get; set; }

        /// <summary>
        /// Gets or Sets Field Constraint Rule count
        /// </summary>
        public int? ConstraintRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets Answer Rule Id
        /// </summary>
        public int? AnswerRuleId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule definition data
        /// </summary>
        [ForeignKey("ConstraintRuleId")]
        public virtual Rule Rules { get; set; }

        /// <summary>
        /// Gets or Sets the Rule definition data
        /// </summary>
        [ForeignKey(" AnswerRuleId")]
        public virtual Rule Rule { get; set; }

        /// <summary>
        /// Gets or Sets Answer Rule Count
        /// </summary>
        public int? AnswerRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets the Created User Id
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets Last Modified Date
        /// </summary>
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets Last Modified User Id
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Gets or Sets the ToBeRedacted
        /// </summary>
        public string ToBeRedacted { get; set; }
    }
}