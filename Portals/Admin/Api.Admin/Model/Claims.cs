using Newtonsoft.Json;
using System.Collections.Generic;

namespace Api.Admin.Model
{
    /// <summary>
    /// claims class to manage claims in toekn
    /// </summary>
    public class Claims
    {
        /// <summary>
        /// Gets or Sets the user permissions
        /// </summary>
        [JsonProperty("groups")]
        public IEnumerable<string> Groups { get; set; }
    }
}