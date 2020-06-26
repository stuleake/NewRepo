using System;
using System.Collections.Generic;

namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// A Command class for Fee calculator rules
    /// </summary>
    public class FeeCalculatorRulesDetails
    {
        /// <summary>
        /// Gets or Sets the Uploaded File Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or Sets the fee calculator applications
        /// </summary>
        public ICollection<RuleDefModel> Rules { get; set; }

        /// <summary>
        /// Gets or Sets the User Id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Master Parameters of Fee Calculator rules
        /// </summary>
        public IDictionary<string, string> MasterParameters { get; set; }
    }
}