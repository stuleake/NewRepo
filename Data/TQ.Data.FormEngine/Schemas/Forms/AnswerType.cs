using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Answer types
    /// </summary>
    [Table("AnswerTypes", Schema = FormEngineSchemas.Forms)]
    public class AnswerType
    {
        /// <summary>
        /// Gets or Sets the Answer Type Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Answer Type
        /// </summary>
        public string AnswerTypes { get; set; }
    }
}