using Newtonsoft.Json;
using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    /// class to manage field Depends
    /// </summary>
    public class Depends
    {
        /// <summary>
        /// Gets or Sets List of answer guide in options
        /// </summary>
        [JsonProperty("conditions", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Condition> Conditions { get; set; }

        /// <summary>
        /// Gets or Sets Conditions To Pass constraints
        /// </summary>
        [JsonProperty("conditionsToPass", NullValueHandling = NullValueHandling.Ignore)]
        public string ConditionsToPass { get; set; }
    }
}