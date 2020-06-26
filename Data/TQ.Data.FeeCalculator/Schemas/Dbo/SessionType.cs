using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model class for Session types
    /// </summary>
    [Table("SessionTypes", Schema = FeeCalculatorSchemas.Dbo)]
    public class SessionType
    {
        /// <summary>
        /// Gets or Sets the Session Type Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SessionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Session Type Name
        /// </summary>
        public string Name { get; set; }
    }
}