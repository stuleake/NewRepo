using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of QSTypes
    /// </summary>
    [Table("QSTypes", Schema = FormEngineSchemas.Forms)]
    public class QSType
    {
        /// <summary>
        /// Gets or Sets the QSType Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QSTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the QSTypes
        /// </summary>
        public string QSTypes { get; set; }
    }
}