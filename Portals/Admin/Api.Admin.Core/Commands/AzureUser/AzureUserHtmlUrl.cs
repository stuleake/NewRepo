using Api.Admin.Core.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Api.Admin.Core.Commands.AzureUser
{
    /// <summary>
    /// Manage Azure User For html url
    /// </summary>
    public class AzureUserHtmlUrl : IRequest<IEnumerable<AzureUserObject>>
    {
        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the active user
        /// </summary>
        public bool Active { get; set; }
    }
}