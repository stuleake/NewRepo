using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model class for Parameter types
    /// </summary>
    [Table("ParameterTypes", Schema = FeeCalculatorSchemas.Dbo)]
    public class ParameterType
    {
        /// <summary>
        /// Gets or Sets the Parameter Type Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ParameterTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter Type Name
        /// </summary>
        public string Name { get; set; }
    }
}