using Api.FeeCalculator.Core.Commands.FeeCalculator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Exceptions;
using TQ.Core.Filters;

namespace Api.FeeCalculator.Controllers
{
    /// <summary>
    /// Controller to import fee calculator rules for applications.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FeeCalculatorRulesController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeCalculatorRulesController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediatR being passed using dependency injection</param>
        public FeeCalculatorRulesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// A Api to import the fee calculator rules.
        /// </summary>
        /// <param name="command">A request object containing rules</param>
        /// <param name="country">Country Request Header.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [Route("import")]
        [CustomAuthorize(RoleTypes.PP2SuperAdmin)]
        public async Task<ActionResult<bool>> CreateFeeCalculatorRulesAsync([FromBody]FeeCalculatorRules command, [FromHeader(Name = RequestHeaderConstants.Country)]string country)
        {
            if (command == null)
            {
                throw new TQException("Null request");
            }
            command.Country = country;
            command.UserId = this.CurrentUser;
            var response = await this.mediator.Send(command);
            return CreatedAtAction(nameof(CreateFeeCalculatorRulesAsync), response);
        }
    }
}