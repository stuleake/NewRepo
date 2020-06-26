using MediatR;
using Newtonsoft.Json;

namespace Api.Admin.Core.Commands.AzureUser
{
    /// <summary>
    /// Azure User Groups creation model
    /// </summary>
    public class AzureUserGroupModel : IRequest<bool>
    {
        /// <summary>
        /// Gets or Sets the display name for group.
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or Sets the mail Nick name for group.
        /// </summary>
        [JsonProperty("mailNickname")]
        public string MailNickname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or Sets the mail Enabled for group.
        /// </summary>
        [JsonProperty("mailEnabled")]
        public bool MailEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or Sets the security Enabled for group.
        /// </summary>
        [JsonProperty("securityEnabled")]
        public bool SecurityEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Description for group.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}