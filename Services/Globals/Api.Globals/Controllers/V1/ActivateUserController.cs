using Api.Globals.Core.Commands.ActivateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Globals.Controllers.V1
{
    /// <summary>
    /// Activate user controller
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]/v1/")]
    [ApiController]
    public class ActivateUserController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivateUserController"/> class
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public ActivateUserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Activate azure user
        /// </summary>
        /// <param name="request">object of Activate User Request </param>
        /// <response code="200">If successfully returned the Activate user response</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>Returns bool value</returns>
        [HttpPost]
        [Route("ActivateUserAsync")]
        [Produces(typeof(bool))]
        public async Task<ActionResult<bool>> ActivateUserAsync(ActivateUserRequest request)
        {
            var activate = await this.mediator.Send(request);
            return activate;
        }
    }
}