using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.Data.FeeCalculator.Schemas.Dbo
{
    /// <summary>
    /// A Db model holding Rule Categories
    /// </summary>
    [Table("Categories", Schema = FeeCalculatorSchemas.Dbo)]
    public class Category
    {
        /// <summary>
        /// Gets or Sets the category id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets the category name
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or Sets the category sequence
        /// </summary>
        public int Sequence { get; set; }
    }
}