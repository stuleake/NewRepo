using TQ.Core.Filters;

namespace Api.Admin.Core.Commands
{
    /// <summary>
    /// Base Command Class
    /// </summary>
    public class BaseCommand
    {
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