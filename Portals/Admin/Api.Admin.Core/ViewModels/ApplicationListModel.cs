using System.Collections.Generic;

namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// A response model class to list the application of a user.
    /// </summary>
    public class ApplicationListModel
    {
        /// <summary>
        /// Gets or Sets the type of applications.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets the Country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets the list of application for a user.
        /// </summary>
        public IEnumerable<UserApplication> Applications { get; set; }
    }
}