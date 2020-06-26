using System;

namespace CT.Utils.Extensions
{
    /// <summary>
    /// Class to handle exception extensions
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Convert int to Enum
        /// </summary>
        /// <typeparam name="T">The target enum type</typeparam>
        /// <param name="value">The source value to be converted</param>
        /// <returns>Returns the desired enum converted from the source integer value</returns>
        public static T ToEnum<T>(this int value)
        {
            var name = Enum.GetName(typeof(T), value);
            return name.ToEnum<T>();
        }
    }
}