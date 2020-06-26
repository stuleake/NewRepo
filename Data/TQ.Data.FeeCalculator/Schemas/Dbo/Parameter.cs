using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model for Parameters for Rules input
    /// </summary>
    [Table("Parameters", Schema = FeeCalculatorSchemas.Dbo)]
    public class Parameter
    {
        /// <summary>
        /// Gets or Sets the Parameter Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ParameterId { get; set; }

        /// <summary>
        /// Gets or Sets the parameter Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Datatype Id
        /// </summary>
        public Guid ParamDataTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the DataType
        /// </summary>
        [ForeignKey("ParamDataTypeId")]
        public ParamDataType ParamDataType { get; set; }

        /// <summary>
        /// Gets or Sets the Tenant
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or Sets the Product
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or Sets the Master value of parameter
        /// </summary>
        public string MasterValue { get; set; }

        /// <summary>
        /// Gets or Sets the list of RuleDef and Parameter Mappings
        /// </summary>
        public IList<RuleDefParameterMapping> RuleDefParameterMappings { get; set; }
    }
}