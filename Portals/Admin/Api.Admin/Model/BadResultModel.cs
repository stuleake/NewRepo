namespace Api.Admin.Model
{
    /// <summary>
    /// A class to Handle Bad Request
    /// </summary>
    public class BadResultModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether operation is successful or not
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the Error message for an operation
        /// </summary>
        public string Message { get; set; }
    }
}