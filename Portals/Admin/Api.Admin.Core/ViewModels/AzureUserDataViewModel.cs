using System.Collections.Generic;

namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// A AzureUser Model to get existing Azure user data in object format
    /// </summary>
    public class AzureUserDataViewModel
    {
        /// <summary>
        /// Gets or Sets the data of Azure user
        /// </summary>
        public IEnumerable<AzureUserObject> Value { get; set; }
    }
}