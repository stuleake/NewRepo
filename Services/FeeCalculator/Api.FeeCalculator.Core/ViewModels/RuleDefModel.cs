using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Api.FeeCalculator.Core.ViewModels
{
    /// <summary>
    /// A model class for Rule Definition
    /// </summary>
    public class RuleDefModel
    {
        /// <summary>
        /// Gets or Sets the Rule id
        /// </summary>
        [JsonPropertyName("reference_id")]
        public int RuleId { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Name
        /// </summary>
        [JsonPropertyName("name")]
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Category
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or Sets the Start date of rule
        /// </summary>
        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or Sets the End date of the rule
        /// </summary>
        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definition
        /// </summary>
        [JsonPropertyName("definition")]
        public string RuleDefinition { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Questions
        /// </summary>
        [JsonPropertyName("parameters")]
        public ICollection<ParameterModel> Parameters { get; set; }
    }
}