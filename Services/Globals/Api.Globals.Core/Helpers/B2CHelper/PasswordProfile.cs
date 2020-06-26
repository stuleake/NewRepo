using Newtonsoft.Json;

namespace Api.Globals.Core.Helpers
{
    /// <summary>
    /// A Class to define password profile
    /// </summary>
    public class PasswordProfile
    {
        /// <summary>
        /// Gets or Sets Password
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ForceChangePasswordNextLogin is true or false
        /// </summary>
        [JsonProperty("forceChangePasswordNextLogin")]
        public bool ForceChangePasswordNextLogin { get; set; }
    }
}