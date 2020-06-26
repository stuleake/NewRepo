using System;
using TQ.Core.Filters;

namespace Api.FormEngine.Core.Commands
{
    /// <summary>
    /// Base Command Class
    /// </summary>
    public class BaseCommand
    {
        /// <summary>
        /// Gets or Sets the country of the User
        /// </summary>
        [SwaggerIgnore]
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets the User Id of the User
        /// </summary>
        [SwaggerIgnore]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets the portal
        /// </summary>
        [SwaggerIgnore]
        public string Portal { get; set; }

        /// <summary>
        /// Gets or Sets the product
        /// </summary>
        [SwaggerIgnore]
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the auth token for the portal
        /// </summary>
        [SwaggerIgnore]
        public string AuthToken { get; set; }
    }
}