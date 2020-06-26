using Api.Planner.Core.Commands.AddressSearch;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Planner.Controllers
{
    /// <summary>
    /// APi to search the Geocoding database for addresses
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AddressSearchController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressSearchController"/> class
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public AddressSearchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get method to list simple addresses
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <param name="country">the country to search for</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        [Route("simple/by-postcode")]
        [Produces(typeof(IEnumerable<SimpleAddressSearchModel>))]
        public async Task<IEnumerable<SimpleAddressSearchModel>> GetSimpleAddressByPostcodeAsync(string postcode, string country)
        {
            var request = new GetSimpleAddressByPostcodeCommand
            {
                Country = country,
                Postcode = postcode
            };

            var response = await mediator.Send(request);

            return response;
        }

        /// <summary>
        /// Get method to list full addresses
        /// </summary>
        /// <param name="postcode">the postcode to search for</param>
        /// <param name="country">the country to search for</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        [Route("full/by-postcode")]
        [Produces(typeof(IEnumerable<FullAddressSearchModel>))]
        public async Task<IEnumerable<FullAddressSearchModel>> GetFullAddressByPostcodeAsync(string postcode, string country)
        {
            var request = new GetFullAddressByPostcodeCommand
            {
                Country = country,
                Postcode = postcode
            };

            var response = await mediator.Send(request);

            return response;
        }
    }
}