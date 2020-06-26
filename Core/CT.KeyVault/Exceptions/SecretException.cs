namespace CT.KeyVault.Exceptions
{
    /// <inheritdoc/>
    public sealed class SecretException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretException"/> class.
        /// </summary>
        /// <param name="message">Custom message for the exception</param>
        /// <param name="innerException">Exception to be embedded as inner exception</param>
        public SecretException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}