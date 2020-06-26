using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace Api.Admin.Core.Commands.FeeCalculator
{
    /// <summary>
    /// Command for importing rules
    /// </summary>
    public class ImportFeeCalculationRules : BaseCommand, IRequest<bool>
    {
        /// <summary>
        /// Gets or Sets the file
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Gets or Sets the User Id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Country of User.
        /// </summary>
        public string Country { get; set; }
    }
}