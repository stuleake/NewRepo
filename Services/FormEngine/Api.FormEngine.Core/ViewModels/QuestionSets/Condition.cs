using Newtonsoft.Json;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    /// Model for Conditions
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// Gets or Sets On
        /// </summary>
        [JsonProperty("on")]
        public string On { get; set; }

        /// <summary>
        /// Gets or Sets value
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}