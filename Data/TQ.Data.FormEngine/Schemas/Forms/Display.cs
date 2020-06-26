using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Field Display
    /// </summary>
    [Table("Displays", Schema = FormEngineSchemas.Forms)]
    public class Display
    {
        /// <summary>
        /// Gets or Sets the Display id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DisplayId { get; set; }

        /// <summary>
        /// Gets or Sets the Display
        /// </summary>
        public string Displays { get; set; }
    }
}