using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TQ.Core.Extensions
{
    /// <summary>
    /// Enum results
    /// </summary>
    internal class EnumResults
    {
        /// <summary>
        /// Gets or Sets display attribute
        /// </summary>
        public DisplayAttribute DisplayAttribute { get; set; }

        /// <summary>
        /// Gets or Sets of field info
        /// </summary>
        public FieldInfo FieldInfo { get; set; }
    }
}