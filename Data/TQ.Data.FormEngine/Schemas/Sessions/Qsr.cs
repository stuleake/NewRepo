using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TQ.Data.FormEngine.Schemas.Forms;

namespace TQ.Data.FormEngine.Schemas.Sessions
{
    /// <summary>
    /// A Database model object of QS Response
    /// </summary>
    [Table("QSR", Schema = FormEngineSchemas.Sessions)]
    public class Qsr
    {
        /// <summary>
        /// Gets or Sets QuestionSet Response id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QsrId { get; set; }

        /// <summary>
        /// Gets or Sets QuestionSet Id
        /// </summary>
        public Guid QSId { get; set; }

        /// <summary>
        /// Gets or Sets the QS definition data
        /// </summary>
        [ForeignKey("QSId")]
        public virtual QS QuestionSet { get; set; }

        /// <summary>
        /// Gets or Sets QuestionSet Number
        /// </summary>
        public int QSNo { get; set; }

        /// <summary>
        /// Gets or Sets the QuestionSet Version
        /// </summary>
        public string QSVersion { get; set; }

        /// <summary>
        /// Gets or Sets QS Collection Id
        /// </summary>
        public Guid QSCollectionId { get; set; }

        /// <summary>
        /// Gets or Sets the QS definition data
        /// </summary>
        [ForeignKey("QSCollectionId")]
        public virtual QSCollection QSCollection { get; set; }

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
    }
}