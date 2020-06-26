using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model class for Output Operation types
    /// </summary>
    [Table("OutputOperationTypes", Schema = FeeCalculatorSchemas.Dbo)]
    public class OutputOperationType
    {
        /// <summary>
        /// Gets or Sets the Output Operation Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OutputOperationId { get; set; }

        /// <summary>
        /// Gets or Sets the Output Operation Name
        /// </summary>
        public string Name { get; set; }
    }
}