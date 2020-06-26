using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TQ.Data.FormEngine.Schemas.Forms;

namespace TQ.Data.FormEngine.Schemas.Sessions
{
    /// <summary>
    ///  A Database model object of QS Collection
    /// </summary>
    [Table("QSCollections", Schema = FormEngineSchemas.Sessions)]
    public class QSCollection
    {
        /// <summary>
        /// Gets or Sets QuestionSet Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QSCollectionId { get; set; }

        /// <summary>
        /// Gets or Sets User Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or Sets Ref No
        /// </summary>
        public string RefNo { get; set; }

        /// <summary>
        /// Gets or Sets QS Collection type Id
        /// </summary>
        public Guid QSCollectionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the QS Collection Type definition data
        /// </summary>
        [ForeignKey("QSCollectionTypeId")]
        public virtual QSCollectionType QSCollectionType { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        public string Status { get; set; }

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
        /// Gets or Sets Last Modified User Id
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets QSCollectionVersion
        /// </summary>
        public double QSCollectionVersion { get; set; }

        /// <summary>
        /// Gets or sets ApplicationTypeRefNo
        /// </summary>
        public int ApplicationTypeRefNo { get; set; }
    }
}