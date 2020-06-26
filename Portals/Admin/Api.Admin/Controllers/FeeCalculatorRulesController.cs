using Api.Admin.Core.Commands.FeeCalculator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Exceptions;
using TQ.Core.Filters;

namespace Api.Admin.Controllers
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
        /// <param name="authorizationToken">Authorization Token request header.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [Route("import")]
        [Produces(typeof(bool))]
        [CustomAuthorize(RoleTypes.PP2SuperAdmin)]
        public async Task<ActionResult<bool>> CreateFeeCalculatorRulesAsync(
            [FromForm]ImportFeeCalculationRules command,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken)
        {
            if (command == null || !command.File.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                throw new TQException("Invalid file");
            }
            command.AuthToken = authorizationToken;
            command.UserId = this.CurrentUser;
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(CreateFeeCalculatorRulesAsync), response);
        }
    }
}