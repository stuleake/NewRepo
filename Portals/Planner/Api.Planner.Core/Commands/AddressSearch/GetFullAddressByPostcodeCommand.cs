using Api.Planner.Core.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Planner.Core.Commands.AddressSearch
{
    /// <summary>
    /// The Get full address by postcode command
    /// </summary>
    public class GetFullAddressByPostcodeCommand : BaseCommand, IRequest<IEnumerable<FullAddressSearchModel>>
    {
        /// <summary>
        /// Gets or sets the postcode to search for
        /// </summary>
        [Required]
        public string Postcode { get; set; }
    }
}