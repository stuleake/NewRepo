using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TQ.Data.FormEngine.Schemas.Forms;

namespace TQ.Data.FormEngine.Schemas.Sessions
{
    /// <summary>
    /// A Database model object of QRS Answer
    /// </summary>
    [Table("QSRAnswers", Schema = FormEngineSchemas.Sessions)]
    public class QsrAnswer
    {
        /// <summary>
        /// Gets or Sets QSR Answer Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QsrAnswerId { get; set; }

        /// <summary>
        /// Gets or Sets QSR Id
        /// </summary>
        public Guid QsrId { get; set; }

        /// <summary>
        /// Gets or Sets the Qsr definition data
        /// </summary>
        [ForeignKey("QsrId")]
        public virtual Qsr Qsr { get; set; }

        /// <summary>
        /// Gets or Sets Field  Id
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
        public int FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets Answer
        /// </summary>
        public string Answer { get; set; }

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
        /// Gets or sets RowNo
        /// </summary>
        public int RowNo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}