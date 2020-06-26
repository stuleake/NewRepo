using System;

namespace TQ.Core.Exceptions
{
    /// <summary>
    /// NoContent custom exception
    /// </summary>
    public class NoContentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoContentException"/> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public NoContentException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoContentException"/> class.
        /// </summary>
        /// <param name="message">Exception Message</param>
        /// <param name="innerException">Inner Exception</param>
        public NoContentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}