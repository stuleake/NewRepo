using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    ///  A Database model object of QS Collection Mappings
    /// </summary>
    [Table("QSCollectionMappings", Schema = FormEngineSchemas.Forms)]
    public class QSCollectionMapping
    {
        /// <summary>
        /// Gets or Sets QS Collection Mapping Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QSCollectionMappingId { get; set; }

        /// <summary>
        /// Gets or Sets QS Collection Type Id
        /// </summary>
        public Guid QSCollectionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Section definition data
        /// </summary>
        [ForeignKey("QSCollectionTypeId")]
        public virtual QSCollectionType QSCollectionType { get; set; }

        /// <summary>
        /// Gets or Sets QS Id
        /// </summary>
        public Guid QSId { get; set; }

        /// <summary>
        /// Gets or Sets the QS definition data
        /// </summary>
        [ForeignKey("QSId")]
        public virtual QS QuestionSet { get; set; }

        /// <summary>
        /// Gets or Sets QS No
        /// </summary>
        public int QSNo { get; set; }

        /// <summary>
        /// Gets or Sets  Sequence
        /// </summary>
        public int Sequence { get; set; }
    }
}