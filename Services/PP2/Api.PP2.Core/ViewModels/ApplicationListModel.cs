using System.Collections.Generic;

namespace Api.PP2.Core.ViewModels
{
    /// <summary>
    /// A model to for list Submitted applications for a user.
    /// </summary>
    public class ApplicationListModel
    {
        /// <summary>
        /// Gets or Sets the type of applications.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets the list of application for a user.
        /// </summary>
        public IEnumerable<UserApplication> Applications { get; set; }
    }
}