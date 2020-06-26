using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A DB model for questionset response
    /// </summary>
    [Table("Qsr", Schema = FeeCalculatorSchemas.Dbo)]
    public class Qsr
    {
        /// <summary>
        /// Gets or Sets the Qsr Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QsrId { get; set; }

        /// <summary>
        /// Gets or Sets the QSCollectionId
        /// </summary>
        public Guid QSCollectionId { get; set; }

        /// <summary>
        /// Gets or Sets the Created Date
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or Sets the User Id creating the Qsr
        /// </summary>
        public Guid CreatedBy { get; set; }
    }
}