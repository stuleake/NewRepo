using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Section Types
    /// </summary>
    [Table("Rules", Schema = FormEngineSchemas.Forms)]
    public class Rule
    {
        /// <summary>
        /// Gets or Sets the Rule id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RuleId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule
        /// </summary>
        public string Rules { get; set; }
    }
}