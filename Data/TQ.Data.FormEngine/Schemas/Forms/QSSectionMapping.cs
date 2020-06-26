using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Question Set, Section mapping
    /// </summary>
    [Table("QSSectionMappings", Schema = FormEngineSchemas.Forms)]
    public class QSSectionMapping
    {
        /// <summary>
        /// Gets or Sets QS Section Mapping Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QSSectionMappingId { get; set; }

        /// <summary>
        /// Gets or Sets QuestionSet id
        /// </summary>
        public Guid QSId { get; set; }

        /// <summary>
        /// Gets or Sets the QS definition data
        /// </summary>
        [ForeignKey("QSId")]
        public virtual QS QuestionSet { get; set; }

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
        /// Gets or Sets the Sequence
        /// </summary>
        public int Sequence { get; set; }
    }
}