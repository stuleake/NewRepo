using Api.FeeCalculator.Core.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Api.FeeCalculator.Core.Commands.FeeCalculator
{
    /// <summary>
    /// A Command class for Fee calculator rules
    /// </summary>
    public class FeeCalculatorRules : BaseCommand, IRequest<bool>
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
        /// Gets or Sets the Master Parameters of Fee Calculator rules
        /// </summary>
        public IDictionary<string, string> MasterParameters { get; set; }
    }
}