using MediatR;
using System.Collections.Generic;

namespace Api.Admin.Core.Commands.AzureUser
{
    /// <summary>
    /// Manage Azure User Group FOR html url
    /// </summary>
    public class AzureUserGroupHtmlUrl : IRequest<IEnumerable<string>>
    {
        /// <summary>
        /// Gets or sets the user object id
        /// </summary>
        public string ObjectId { get; set; }
    }
}