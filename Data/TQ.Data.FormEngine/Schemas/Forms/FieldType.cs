using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Field Types
    /// </summary>
    [Table("FieldTypes", Schema = FormEngineSchemas.Forms)]
    public class FieldType
    {
        /// <summary>
        /// Gets or Sets the Rule id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FieldTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule
        /// </summary>
        public string FieldTypes { get; set; }
    }
}