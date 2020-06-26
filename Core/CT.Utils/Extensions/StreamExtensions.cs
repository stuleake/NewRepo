using System.IO;

namespace CT.Utils.Extensions
{
    /// <summary>
    /// Class to handle stream extensions
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Convert the content of the memory stream to string
        /// </summary>
        /// <param name="stream">The source stream</param>
        /// <returns>Returns a string that is converted from the source memory stream</returns>
        public static string ToContentString(this Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}