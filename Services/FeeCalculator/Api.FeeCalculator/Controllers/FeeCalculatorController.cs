using Api.FeeCalculator.Core.Commands.FeeCalculator;
using Api.FeeCalculator.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;

namespace Api.FeeCalculator.Controllers
{
    /// <summary>
    /// A Api controller to calculate Fee
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FeeCalculatorController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeCalculatorController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediatR being passed using dependency injection</param>
        public FeeCalculatorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// A Api to calculate fee for input parameter value
        /// </summary>
        /// <param name="command">A request object containing list of parameter name and value</param>
        /// <param name="country">Country Request Header.</param>
        /// <returns>Response object containing execution step and result</returns>
        [HttpPost]
        [Route("calculator")]
        public async Task<ActionResult<FeeCalculatorResponseModel>> CreateFeeCalculatorDataAsync(FeeCalculatorData command, [FromHeader(Name = RequestHeaderConstants.Country)]string country)
        {
            command.Country = country;
            var response = await mediator.Send(command);
            return response;
        }
    }
}