using Newtonsoft.Json;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    /// Model for Options
    /// </summary>
    public class Option
    {
        /// <summary>
        /// Gets or Sets key
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or Sets value
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}