using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    /// Model for Section
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Gets or Sets Section Id
        /// </summary>
        [JsonProperty("id")]
        public Guid SectionId { get; set; }

        /// <summary>
        /// Gets or Sets the Label value of Section
        /// </summary>
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string SectionLabel { get; set; }

        /// <summary>
        /// Gets or Sets the Helptext value of Section
        /// </summary>
        [JsonProperty("helptext", NullValueHandling = NullValueHandling.Ignore)]
        public string SectionHelptext { get; set; }

        /// <summary>
        /// Gets or Sets the Description value of Section
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string SectionDescription { get; set; }

        /// <summary>
        /// Gets or Sets the Section Type
        /// </summary>
        [JsonIgnore]
        public string SectionType { get; set; }

        /// <summary>
        /// Gets or Sets the Rule
        /// </summary>
        [JsonIgnore]
        public string SectionRule { get; set; }

        /// <summary>
        /// Gets or Sets the Section Rule count
        /// </summary>
        [JsonIgnore]
        public int? SectionRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets the Section Sequence
        /// </summary>
        [JsonIgnore]
        public int SectionSequence { get; set; }

        /// <summary>
        /// Gets or Sets depends
        /// </summary>
        [JsonProperty("depends", NullValueHandling = NullValueHandling.Ignore)]
        public Depends Depends { get; set; }

        /// <summary>
        /// Gets or Sets List of fields
        /// </summary>
        [JsonProperty("fields")]
        public IEnumerable<Field> Fields { get; set; }
    }
}