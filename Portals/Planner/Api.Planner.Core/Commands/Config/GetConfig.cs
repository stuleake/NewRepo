using MediatR;
using System.Collections.Generic;

namespace Api.Planner.Core.Commands.Config
{
    /// <summary>
    /// A class to contain configuration data for azure
    /// </summary>
    public class GetConfig : IRequest<ViewModels.Config>
    {
        /// <summary>
        /// Gets or sets list of secret keys to access the resources from azure
        /// </summary>
        public IEnumerable<string> Keys { get; set; }
    }
}