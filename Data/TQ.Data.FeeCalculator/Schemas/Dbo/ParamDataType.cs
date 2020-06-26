using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model class for datatypes
    /// </summary>
    [Table("ParamDataTypes", Schema = FeeCalculatorSchemas.Dbo)]
    public class ParamDataType
    {
        /// <summary>
        /// Gets or Sets the Datatype Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ParamDataTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Datatype
        /// </summary>
        public string ParamDataTypeName { get; set; }
    }
}