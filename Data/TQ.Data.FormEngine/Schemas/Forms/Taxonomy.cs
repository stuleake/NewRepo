using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model for taxonomy
    /// </summary>
    [Table("Taxonomy", Schema = FormEngineSchemas.Forms)]
    public class Taxonomy
    {
        /// <summary>
        /// Gets or sets Taxonomy Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaxonomyID { get; set; }

        /// <summary>
        /// Gets or sets Product
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets Tenant
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets LanguageCode
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets QsNo
        /// </summary>
        public int QsNo { get; set; }

        /// <summary>
        /// Gets or sets QsVersion
        /// </summary>
        public string QsVersion { get; set; }

        /// <summary>
        /// Gets or sets Taxonomies
        /// </summary>
        public string TaxonomyDictionary { get; set; }
    }
}