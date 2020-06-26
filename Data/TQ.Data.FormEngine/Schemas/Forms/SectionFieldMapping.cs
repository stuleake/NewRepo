using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of  Section, Field  mapping
    /// </summary>
    [Table("SectionFieldMappings", Schema = FormEngineSchemas.Forms)]
    public class SectionFieldMapping
    {
        /// <summary>
        /// Gets or Sets Section Field Mapping Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SectionFieldMappingId { get; set; }

        /// <summary>
        /// Gets or Sets the Section Id
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// Gets or Sets the Section definition data
        /// </summary>
        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }

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
        /// Gets or Sets the FieldNo
        /// </summary>
        public int? FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets the Sequence
        /// </summary>
        public int Sequence { get; set; }
    }
}