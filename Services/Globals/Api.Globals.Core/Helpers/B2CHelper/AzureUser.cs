using Newtonsoft.Json;
using System.Collections.Generic;

namespace Api.Globals.Core.Helpers
{
    /// <summary>
    /// Azure User class to manage Azure Users
    /// </summary>
    public class AzureUser
    {
        /// <summary>
        /// Gets or sets a value indicating whether AccountEnabled
        /// </summary>
        [JsonProperty("accountEnabled")]
        public bool AccountEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the SignInNames
        /// </summary>
        [JsonProperty("signInNames")]
        public IEnumerable<SignInNames> SignInNames { get; set; }

        /// <summary>
        /// Gets or Sets Creation Type
        /// </summary>
        [JsonProperty("creationType")]
        public string CreationType { get; set; }

        /// <summary>
        /// Gets or Sets Display name
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or Sets password profile
        /// </summary>
        [JsonProperty("passwordProfile")]
        public PasswordProfile PasswordProfile { get; set; }

        /// <summary>
        /// Gets or Sets password policies
        /// </summary>
        [JsonProperty("passwordPolicies")]
        public string PasswordPolicies { get; set; }

        /// <summary>
        /// Gets or Sets firstname
        /// </summary>
        [JsonProperty("givenName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets lastname
        /// </summary>
        [JsonProperty("surname")]
        public string Surname { get; set; }
    }
}