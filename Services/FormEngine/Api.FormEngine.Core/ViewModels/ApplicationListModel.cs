using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels
{
    /// <summary>
    /// A response model class to list the draft applications of a User.
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