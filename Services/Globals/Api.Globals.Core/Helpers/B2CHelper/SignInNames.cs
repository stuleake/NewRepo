using Newtonsoft.Json;

namespace Api.Globals.Core.Helpers
{
    /// <summary>
    /// Class to define singin names
    /// </summary>
    public class SignInNames
    {
        /// <summary>
        /// Gets or Sets Singin type
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Singin Value
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}