using Newtonsoft.Json;

namespace Api.PP2.Model
{
    /// <summary>
    /// A class to create ErrorDetails in case of Exception
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or Sets the Status code for Api
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or Sets the Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or Sets the Operation Id
        /// </summary>
        public string OperationId { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}