using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Answer Guide
    /// </summary>
    [Table("AnswerGuides", Schema = FormEngineSchemas.Forms)]
    public class AnswerGuide
    {
        /// <summary>
        /// Gets or Sets Answer Guide Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AnswerGuideId { get; set; }

        /// <summary>
        /// Gets or Sets Field Id
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or Sets the Field definition data
        /// </summary>
        [ForeignKey("FieldId")]
        public virtual Field Field { get; set; }

        /// <summary>
        /// Gets or Sets Answer Type Id
        /// </summary>
        public int AnswerTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the AnswerTypes definition data
        /// </summary>
        [ForeignKey("AnswerTypeId")]
        public virtual AnswerType AnswerTypes { get; set; }

        /// <summary>
        /// Gets or Sets the min
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// Gets or Sets the max
        /// </summary>
        public string Max { get; set; }

        /// <summary>
        /// Gets or Sets the Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets the isDefault
        /// </summary>
        public string IsDefault { get; set; }

        /// <summary>
        /// Gets or Sets the Sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or Sets the Error label
        /// </summary>
        public string ErrLabel { get; set; }

        /// <summary>
        /// Gets or Sets the AnswerGuideNo
        /// </summary>
        public int AnswerGuideNo { get; set; }

        /// <summary>
        /// Gets or Sets the Copy From Field No
        /// </summary>
        public int? CopyFromFieldNo { get; set; }

        /// <summary>
        /// Gets or Sets the Copy From QS No
        /// </summary>
        public int? CopyFromQSNo { get; set; }
    }
}