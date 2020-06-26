using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A database model of Fee calculator Test set header
    /// </summary>
    [Table("TestSetHeaders", Schema = FeeCalculatorSchemas.Dbo)]
    public class TestSetHeader
    {
        /// <summary>
        /// Gets or Sets the Fee calculator test header id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TestSetHeaderId { get; set; }

        /// <summary>
        /// Gets or Sets the name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or Sets the Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets the User Id that modified the Test Set Header
        /// </summary>
        public Guid ModifiedBy { get; set; }
    }
}