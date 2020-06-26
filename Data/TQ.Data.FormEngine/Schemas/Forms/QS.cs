using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Question Set
    /// </summary>
    [Table("QS", Schema = FormEngineSchemas.Forms)]
    public class QS
    {
        /// <summary>
        /// Gets or Sets QuestionSet Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QSId { get; set; }

        /// <summary>
        /// Gets or Sets QuestionSet Number
        /// </summary>
        public int QSNo { get; set; }

        /// <summary>
        /// Gets or Sets Tenant
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or Sets Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or Sets the QuestionSet Version
        /// </summary>
        [Column(TypeName = "decimal(18,1)")]
        public decimal QSVersion { get; set; }

        /// <summary>
        /// Gets or Sets the QuestionSet Name
        /// </summary>
        public string QSName { get; set; }

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
        /// Gets or Sets the Status Id
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or Sets the Status Type definition data
        /// </summary>
        [ForeignKey("StatusId")]
        public virtual Statuses Status { get; set; }

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
        /// Gets or Sets the Status Id
        /// </summary>
        public int? QSTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Status Type definition data
        /// </summary>
        [ForeignKey("QSTypeId")]
        public virtual QSType QSType { get; set; }

        /// <summary>
        /// Gets or Sets the File Location
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// Gets or Sets the Taxonomy Location
        /// </summary>
        public string TaxonomyLocation { get; set; }

        /// <summary>
        /// Gets or Sets  Product
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or Sets warning message
        /// </summary>
        public string WarningMessage { get; set; }
    }
}