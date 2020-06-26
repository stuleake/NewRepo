using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Section Types
    /// </summary>
    [Table("SectionTypes", Schema = FormEngineSchemas.Forms)]
    public class SectionType
    {
        /// <summary>
        /// Gets or Sets the SectionType id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Section Type
        /// </summary>
        public string SectionTypes { get; set; }
    }
}