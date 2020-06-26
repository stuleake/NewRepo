using System;
using System.IO;

namespace CT.Utils.Extensions
{
    /// <summary>
    /// Class to handle string extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Provides a sequence of bytes
        /// </summary>
        /// <param name="str">String input to convert into a stream</param>
        /// <returns>Returns a sequence of bytes from the given string</returns>
        public static Stream ToStream(this string str)
        {
            var result = new MemoryStream();

            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(str);
                writer.Flush();
                stream.Position = 0;
                stream.CopyTo(result);
                result.Position = 0;
            }

            return result;
        }

        /// <summary>
        /// Convert string to Enum
        /// </summary>
        /// <typeparam name="T">The target enum type</typeparam>
        /// <param name="value">The source string to be converted</param>
        /// <returns>Returns the desired enum converted from the source string</returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Format the string to insert the parameters
        /// </summary>
        /// <param name="value">The source string</param>
        /// <param name="parameters">The object parameters to be placed in the string</param>
        /// <returns>Returns a string with the added object parameters</returns>
        public static string ToFormat(this string value, params object[] parameters)
        {
            return string.Format(value, parameters);
        }

        /// <summary>
        /// Convert the string to Title Case
        /// </summary>
        /// <param name="value">The source string to be converted</param>
        /// <returns>Returns the string with a Title case</returns>
        public static string ToTitleCase(this string value)
        {
            var textInfo = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(value);
        }
    }
}