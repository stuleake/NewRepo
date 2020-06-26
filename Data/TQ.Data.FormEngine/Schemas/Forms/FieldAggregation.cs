using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FormEngine.Schemas.Forms
{
    /// <summary>
    /// A Database model object of Field Aggregations
    /// </summary>
    [Table("FieldAggregations", Schema = FormEngineSchemas.Forms)]
    public class FieldAggregation
    {
        /// <summary>
        /// Gets or Sets Field Aggregation Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FieldAggregationId { get; set; }

        /// <summary>
        /// Gets or Sets Field Id
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or Sets the Field definition data
        /// </summary>
        [ForeignKey("FieldId")]
        public virtual Field Field { get; set; }

        /// <summary>
        /// Gets or Sets Aggregated Field Id
        /// </summary>
        public Guid AggregatedFieldId { get; set; }

        /// <summary>
        /// Gets or Sets Function Id
        /// </summary>
        public int FunctionId { get; set; }

        /// <summary>
        /// Gets or Sets the Field definition data
        /// </summary>
        [ForeignKey("FunctionId")]
        public virtual Function Function { get; set; }

        /// <summary>
        /// Gets or Sets Sequence
        /// </summary>
        public int Sequence { get; set; }
    }
}