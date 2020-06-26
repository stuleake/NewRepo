using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// A model class for Rule Definition
    /// </summary>
    public class RuleDefModel
    {
        /// <summary>
        /// Gets or Sets the Rule id
        /// </summary>
        [JsonProperty("reference_id")]
        public int RuleId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Category
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or Sets the Start date of rule
        /// </summary>
        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or Sets the End date of the rule
        /// </summary>
        [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definition
        /// </summary>
        [JsonProperty("definition")]
        public string Definition { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Questions
        /// </summary>
        [JsonProperty("parameters")]
        public ICollection<ParameterModel> Parameters { get; set; }
    }
}