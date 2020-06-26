using System;

namespace TQ.Core.Exceptions
{
    /// <summary>
    /// A Exception Class to handle Service Exception
    /// </summary>
    public class MappingResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingResponseException"/> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public MappingResponseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingResponseException"/> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public MappingResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}