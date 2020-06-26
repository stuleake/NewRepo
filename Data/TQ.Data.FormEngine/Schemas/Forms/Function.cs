using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Functions
    /// </summary>
    [Table("Functions", Schema = FormEngineSchemas.Forms)]
    public class Function
    {
        /// <summary>
        /// Gets or Sets the Function id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FunctionsId { get; set; }

        /// <summary>
        /// Gets or Sets the Function
        /// </summary>
        public string Functions { get; set; }
    }
}