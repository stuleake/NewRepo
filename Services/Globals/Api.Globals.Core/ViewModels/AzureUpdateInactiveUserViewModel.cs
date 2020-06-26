using Newtonsoft.Json;

namespace Api.Globals.Core.ViewModels
{
    /// <summary>
    /// Update Inactive user
    /// </summary>
    internal class AzureUpdateInactiveUserViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether account is enabled or not
        /// </summary>
        [JsonProperty("accountEnabled")]
        public bool AccountEnabled { get; set; }
    }
}