using MediatR;
using Newtonsoft.Json;

namespace Api.Planner.Core.Commands.Gatsby
{
    /// <summary>
    /// Model for Gatsby Definition
    /// </summary>
    public class GatsbyDefinition : IRequest<bool>
    {
        /// <summary>
        /// Gets or Sets Definition
        /// </summary>
        [JsonProperty("definition")]
        public Definition Definition { get; set; }
    }
}