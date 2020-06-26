using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of QS Collection Type
    /// </summary>
    [Table("QSCollectionTypes", Schema = FormEngineSchemas.Forms)]
    public class QSCollectionType
    {
        /// <summary>
        /// Gets or Sets the QS Collection Type Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QSCollectionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets the Helptext
        /// </summary>
        public string Helptext { get; set; }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Country Code
        /// </summary>
        public string CountryCode { get; set; }

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
        /// Gets or Sets the Last Modified User Id
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets  Product
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or Sets Tenant
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets ApplicationTypeRefNo
        /// </summary>
        public int ApplicationTypeRefNo { get; set; }

        /// <summary>
        /// Gets or sets QSCollectionVersion
        /// </summary>
        public double QSCollectionVersion { get; set; }
    }
}