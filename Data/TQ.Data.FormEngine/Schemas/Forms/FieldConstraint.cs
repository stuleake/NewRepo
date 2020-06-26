using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Field Constraints
    /// </summary>
    [Table("FieldConstraints", Schema = FormEngineSchemas.Forms)]
    public class FieldConstraint
    {
        /// <summary>
        /// Gets or Sets Field Constraint Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FieldConstraintId { get; set; }

        /// <summary>
        /// Gets or Sets Field  Id
        /// </summary>
        public Guid? FieldId { get; set; }

        /// <summary>
        /// Gets or Sets the Field definition data
        /// </summary>
        [ForeignKey("FieldId")]
        public virtual Field Field { get; set; }

        /// <summary>
        /// Gets or Sets Dependant Answer Guide Id
        /// </summary>
        public Guid? DependantAnswerGuideId { get; set; }

        /// <summary>
        /// Gets or Sets Section Id
        /// </summary>
        public Guid? SectionId { get; set; }

        /// <summary>
        /// Gets or Sets Section No
        /// </summary>
        public int? SectionNo { get; set; }

        /// <summary>
        /// Gets or Sets Dependant Answer Guide No
        /// </summary>
        public int? DependantAnswerGuideNo { get; set; }

        /// <summary>
        /// Gets or Sets Dependant Answer Guide QS No
        /// </summary>
        public int? DependantAnswerQSNo { get; set; }
    }
}