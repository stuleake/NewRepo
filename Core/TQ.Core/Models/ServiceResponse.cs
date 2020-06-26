using Newtonsoft.Json;

namespace TQ.Core.Models
{
    /// <summary>
    /// Model for Service Response
    /// </summary>
    /// <typeparam name="TEntity">type of object</typeparam>
    [JsonObject]
    public class ServiceResponse<TEntity>
    {
        /// <summary>
        /// Gets or Sets the code
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or Sets Value of T
        /// </summary>
        [JsonProperty("value")]
        public TEntity Value { get; set; }

        /// <summary>
        /// Gets or Sets Operation Id
        /// </summary>
        [JsonProperty("operationid")]
        public string OperationId { get; set; }

        /// <summary>
        /// Gets or Sets message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}