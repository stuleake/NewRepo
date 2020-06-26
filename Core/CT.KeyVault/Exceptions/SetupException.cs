namespace CT.KeyVault
{
    /// <inheritdoc/>
    public sealed class SetupException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupException"/> class.
        /// </summary>
        /// <param name="message">Custom message for the exception</param>
        public SetupException(string message) : base(message)
        {
        }
    }
}