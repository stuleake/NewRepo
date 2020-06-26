using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TQ.Core.Extensions
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringExtensions
    {
        private const string SingleSpace = " ";

        /// <summary>
        /// Checks that the search string is a valid UK postcode
        /// </summary>
        /// <param name="searchString">the string to check</param>
        /// <returns>true if the input is a valid postcode, false otherwise</returns>
        public static bool IsPostCode(this string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return false;
            }

            var searchStringWithoutSpaces = searchString.Replace(SingleSpace, string.Empty, StringComparison.CurrentCulture);

            if (searchStringWithoutSpaces.ToCharArray().Any(c => !char.IsLetter(c) && !char.IsNumber(c)))
            {
                return false;
            }

            return IsValidPostCodeWithoutSpace(searchStringWithoutSpaces);
        }

        /// <summary>
        /// Checks that the search string is a valid UK postcode but without the space
        /// </summary>
        /// <param name="searchString">the string to check</param>
        /// <returns>true if the input is a valid postcode but without the space, false otherwise</returns>
        private static bool IsValidPostCodeWithoutSpace(this string searchString)
        {
            return Regex.IsMatch(
                searchString,
                "^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))[0-9][A-Za-z]{2})$");
        }
    }
}