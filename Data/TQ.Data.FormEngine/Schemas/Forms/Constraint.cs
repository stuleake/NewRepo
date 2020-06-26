using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Field Constraints
    /// </summary>
    [Table("Constraints", Schema = FormEngineSchemas.Forms)]
    public class Constraint
    {
        /// <summary>
        /// Gets or Sets the Constraint id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConstraintId { get; set; }

        /// <summary>
        /// Gets or Sets the Constraint
        /// </summary>
        public string Constraints { get; set; }
    }
}