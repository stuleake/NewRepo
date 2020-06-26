using Newtonsoft.Json;
using System.Collections.Generic;

namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// Rules for application typesa
    /// </summary>
    public class RuleApplicationModel
    {
        /// <summary>
        /// Gets or sets the application Type Reference Id
        /// </summary>
        [JsonProperty("applicationtype_id")]
        public int ApplicationTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Application name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the creatyed by for the rule
        /// </summary>
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets the Fee question sets
        /// </summary>
        [JsonProperty("fee_questionsets")]
        public ICollection<int> FeeQuestionSets { get; set; }

        /// <summary>
        /// Gets or sets the list of rules application for the application type
        /// </summary>
        [JsonProperty("serviceChargeRules")]
        public IEnumerable<int> Rules { get; set; }
    }
}