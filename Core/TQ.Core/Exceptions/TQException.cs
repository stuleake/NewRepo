using System;

namespace TQ.Core.Exceptions
{
    /// <summary>
    /// Custom Exception class
    /// </summary>
    public class TQException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TQException"/> class.
        /// </summary>
        /// <param name="msg">Exception message</param>
        public TQException(string msg) : base(msg)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TQException"/> class.
        /// </summary>
        /// <param name="message">Exception Message</param>
        /// <param name="innerException">Inner Exception</param>
        public TQException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}