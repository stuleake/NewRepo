using System.ComponentModel.DataAnnotations;

namespace TQ.Core.Enums
{
    /// <summary>
    /// Enumeration for the different address types
    /// </summary>
    public enum AddressTypes
    {
        /// <summary>
        /// The Full address type
        /// </summary>
        [Display(Name = "Full")]
        Full = 1,

        /// <summary>
        /// The Simple address type
        /// </summary>
        [Display(Name = "Simple")]
        Simple = 2,
    }
}