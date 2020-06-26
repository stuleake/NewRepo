using System;
using TQ.Core.Filters;

namespace Api.Planner.Core.Commands
{
    /// <summary>
    /// Base Class
    /// </summary>
    public class BaseCommand
    {
        /// <summary>
        /// Gets or Sets the country of the user
        /// </summary>
        [SwaggerIgnore]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets User Id of the Logged In User
        /// </summary>
        [SwaggerIgnore]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets Authentication Token
        /// </summary>
        [SwaggerIgnore]
        public string AuthToken { get; set; }

        /// <summary>
        /// Gets or Sets the product
        /// </summary>
        [SwaggerIgnore]
        public string Product { get; set; }
    }
}