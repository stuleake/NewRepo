using Api.Globals.Core.Helpers;
using System.Collections.Generic;

namespace Api.Globals.Core.ViewModels
{
    /// <summary>
    /// A model for Azure User object
    /// </summary>
    public class AzureUserObject
    {
        /// <summary>
        /// Gets or Sets object Id of Azure User
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or Sets the Emailid
        /// </summary>
        public string Emailid { get; set; }

        /// <summary>
        /// Gets or Sets the Display Name of Azure User
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or Sets the User principal name
        /// </summary>
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user account is enabled or not
        /// </summary>
        public bool AccountEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the SignInNames
        /// </summary>
        public IEnumerable<SignInNames> SignInNames { get; set; }
    }
}