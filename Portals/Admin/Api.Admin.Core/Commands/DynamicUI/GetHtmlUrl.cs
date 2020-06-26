using MediatR;

namespace Api.Admin.Core.Commands.DynamicUI
{
    /// <summary>
    /// A model to get Html page Url for country
    /// </summary>
    public class GetHtmlUrl : IRequest<string>
    {
        /// <summary>
        /// Gets or Sets the Country name
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets the root path
        /// </summary>
        public string ContentRootPath { get; set; }
    }
}