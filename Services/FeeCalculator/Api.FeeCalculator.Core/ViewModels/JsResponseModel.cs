using Newtonsoft.Json;

namespace Api.FeeCalculator.Core.ViewModels
{
    /// <summary>
    /// A model class for javascript function execution output
    /// </summary>
    public class JsResponseModel
    {
        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the Rule No
        /// </summary>
        [JsonProperty("ruleNo")]
        public int RuleNo { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter name
        /// </summary>
        [JsonProperty("outputparametername")]
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or Sets the Value
        /// </summary>
        [JsonProperty("outputparametervalue")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets the Datatype
        /// </summary>
        [JsonProperty("outputparameterdatatype")]
        public string Datatype { get; set; }

        /// <summary>
        /// Gets or Sets the Output operation details
        /// </summary>
        [JsonProperty("outputoperation")]
        public string OutPutOperation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output is final or not
        /// </summary>
        [JsonProperty("isFinalOutput")]
        public bool IsFinalOutput { get; set; }
    }
}