using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A database object model of fee calculator definition package
    /// </summary>
    [Table("DefPackages", Schema = FeeCalculatorSchemas.Dbo)]
    public class DefPackage
    {
        /// <summary>
        /// Gets or Sets Fee calculator definition package Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DefPackageId { get; set; }

        /// <summary>
        /// Gets or Sets the Created Date of Package
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets the User Id that created package
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets the File Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or Sets the Status of definition package
        /// </summary>
        public string Status { get; set; }
    }
}