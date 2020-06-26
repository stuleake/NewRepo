using System;
using TQ.Core.Filters;

namespace Api.PP2.Core.Commands
{
    /// <summary>
    /// Class for Base command
    /// </summary>
    public class BaseCommand
    {
        /// <summary>
        /// Gets or Sets the Country
        /// </summary>
        [SwaggerIgnore]
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets the User Id
        /// </summary>
        [SwaggerIgnore]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Authentication token of a user.
        /// </summary>
        public string AuthToken { get; set; }
    }
}