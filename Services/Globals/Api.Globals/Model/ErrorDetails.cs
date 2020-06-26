using Newtonsoft.Json;

namespace Api.Globals.Model
{
    /// <summary>
    /// Class for error details
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or Sets status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or Sets message for error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or Sets operation id
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// overriden ToString method for the class
        /// </summary>
        /// <returns>Json Serialized string of the object </returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}