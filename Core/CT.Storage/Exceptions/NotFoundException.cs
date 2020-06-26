using System;

namespace CT.Storage.Exceptions
{
    /// <summary>
    /// Custom Exception to handle business errors on CT.Storage
    /// </summary>
    public sealed class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="msg">Message for the custom exception</param>
        public NotFoundException(string msg) : base(msg)
        {
        }
    }
}