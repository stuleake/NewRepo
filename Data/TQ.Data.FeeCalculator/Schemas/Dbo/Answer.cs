using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A DB model for Question Asnwer for parameter
    /// </summary>
    [Table("Answers", Schema = FeeCalculatorSchemas.Dbo)]
    public class Answer
    {
        /// <summary>
        /// Gets or Sets the Answer Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AnswerId { get; set; }

        /// <summary>
        /// Gets or Sets the parameter name
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or Sets the session type
        /// </summary>
        public string SessionType { get; set; }

        /// <summary>
        /// Gets or Sets the session Id
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Gets or Sets the Answer for parameter
        /// </summary>
        public string ParameterAnswer { get; set; }

        /// <summary>
        /// Gets or Sets the RowNo of answer
        /// </summary>
        public int RowNo { get; set; }
    }
}