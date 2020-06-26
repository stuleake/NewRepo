using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of QuestionSet Section
    /// </summary>
    [Table("Sections", Schema = FormEngineSchemas.Forms)]
    public class Section
    {
        /// <summary>
        /// Gets or Sets Section Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SectionId { get; set; }

        /// <summary>
        /// Gets or Sets the Label value of Section
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets the Helptext value of Section
        /// </summary>
        public string Helptext { get; set; }

        /// <summary>
        /// Gets or Sets the Description value of Section
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the Section Type Id
        /// </summary>
        public int SectionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Section Type definition data
        /// </summary>
        [ForeignKey("SectionTypeId")]
        public virtual SectionType SectionType { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Id
        /// </summary>
        public int? RuleId { get; set; }

        /// <summary>
        /// Gets or Sets the Rules definition data
        /// </summary>
        [ForeignKey("RuleId")]
        public virtual Rule Rules { get; set; }

        /// <summary>
        /// Gets or Sets the Section Rule count
        /// </summary>
        public int? RuleCount { get; set; }

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
        /// Gets or Sets the SectionNo
        /// </summary>
        public int SectionNo { get; set; }
    }
}