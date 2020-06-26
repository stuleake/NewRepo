using Api.Globals.Core.Commands.LastLogin;
using Api.Globals.Examples;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Globals.Controllers.V1
{
    /// <summary>
    /// Update Lastlogin of user controller
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]/v1/")]
    [ApiController]
    public class LastLoginController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastLoginController"/> class
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public LastLoginController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Update Last login of a user
        /// </summary>
        /// <param name="request">object of Update Last login Request </param>
        /// <response code="200">If successfully returned the last login response</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>Returns boolean value</returns>
        [HttpPut]
        [Route("PutUpdateUserLastLoginAsync")]
        [Produces(typeof(bool))]
        [SwaggerRequestExample(typeof(LastLoginRequest), typeof(LastLoginRequestExample))]
        public async Task<ActionResult<bool>> PutUpdateUserLastLoginAsync(LastLoginRequest request)
        {
            var lastlogin = await this.mediator.Send(request);
            return lastlogin;
        }
    }
}