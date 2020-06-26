using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    /// Model for Questionset Definition
    /// </summary>
    public class QuestionSetDefinition
    {
        /// <summary>
        /// Gets or Sets the Label
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets the Helptext
        /// </summary>
        [JsonProperty("helptext")]
        public string Helptext { get; set; }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets QuestionSet Id
        /// </summary>
        [JsonProperty("id")]
        public Guid QSId { get; set; }

        /// <summary>
        /// Gets or sets List of Sections
        /// </summary>
        [JsonProperty("sections")]
        public IEnumerable<Section> Sections { get; set; }

        /// <summary>
        /// Gets or Sets initialValues
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string> InitialValues { get; set; }
    }
}