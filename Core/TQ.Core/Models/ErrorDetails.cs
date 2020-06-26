using Newtonsoft.Json;

namespace TQ.Core.Models
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

        /// <summary>
        /// overriden ToString method for the class
        /// </summary>
        /// <returns>Josn Serialized string of the object </returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}