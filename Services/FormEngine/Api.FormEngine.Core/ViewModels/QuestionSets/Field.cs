using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    /// Model for Field
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Gets or Sets Field Id
        /// </summary>
        [JsonIgnore]
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or Sets  Filedid-FiledNo
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Field Id
        /// </summary>
        [JsonProperty("name")]
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or Sets the FieldNo
        /// </summary>
        [JsonIgnore]
        public int FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets the FieldVersion
        /// </summary>
        [JsonIgnore]
        public string FieldVersion { get; set; }

        /// <summary>
        /// Gets or Sets the Label
        /// </summary>
        [JsonProperty("label")]
        public string FieldLabel { get; set; }

        /// <summary>
        /// Gets or Sets the Helptext
        /// </summary>
        [JsonProperty("helptext")]
        public string FieldHelptext { get; set; }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        [JsonProperty("description")]
        public string FieldDescription { get; set; }

        /// <summary>
        /// Gets or Sets Field Type
        /// </summary>
        [JsonProperty("fieldtype")]
        public string FieldType { get; set; }

        /// <summary>
        /// Gets or sets Past Date Allowed
        /// </summary>
        [JsonProperty("pastDateAllowed", NullValueHandling = NullValueHandling.Ignore)]
        public string PastDateAllowed { get; set; }

        /// <summary>
        /// Gets or sets Future Date Allowed
        /// </summary>
        [JsonProperty("futureDateAllowed", NullValueHandling = NullValueHandling.Ignore)]
        public string FutureDateAllowed { get; set; }

        /// <summary>
        /// Gets or sets min Date field
        /// </summary>
        [JsonProperty("minDateField", NullValueHandling = NullValueHandling.Ignore)]
        public string MinDateField { get; set; }

        /// <summary>
        /// Gets or sets max Date field
        /// </summary>
        [JsonProperty("maxDateField", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxDateField { get; set; }

        /// <summary>
        /// Gets or Sets Field Display
        /// </summary>
        [JsonIgnore]
        public string Display { get; set; }

        /// <summary>
        /// Gets or Sets Display Constraint
        /// </summary>
        [JsonIgnore]
        public string DisplayConstraint { get; set; }

        /// <summary>
        /// Gets or Sets  Constraint Rule
        /// </summary>
        [JsonIgnore]
        public string ConstraintRule { get; set; }

        /// <summary>
        /// Gets or Sets Field Constraint Rule count
        /// </summary>
        [JsonIgnore]
        public int? ConstraintRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets Answer Rule
        /// </summary>
        [JsonIgnore]
        public string AnswerRule { get; set; }

        /// <summary>
        /// Gets or Sets Answer Rule Count
        /// </summary>
        [JsonIgnore]
        public int? AnswerRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets Field Sequence
        /// </summary>
        [JsonProperty("sequence")]
        public int FieldSequence { get; set; }

        /// <summary>
        /// Gets or Sets depends
        /// </summary>
        [JsonProperty("depends", NullValueHandling = NullValueHandling.Ignore)]
        public Depends Depends { get; set; }

        /// <summary>
        /// Gets or Sets List of answer guide in options
        /// </summary>
        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Option> Options { get; set; }

        /// <summary>
        /// Gets or Sets List of rules
        /// </summary>
        [JsonProperty("rules", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<FieldRule> Rule { get; set; }

        /// <summary>
        /// Gets or Sets List of AnswerGuides
        /// </summary>
        [JsonIgnore]
        public IEnumerable<AnswerGuide> AnswerGuides { get; set; }
    }
}