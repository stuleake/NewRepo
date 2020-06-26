using Api.Planner.Core.Commands.ActivateUser;
using Api.Planner.Core.ViewModels;
using Api.Planner.Example;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Planner.Controllers
{ /// <summary>
  /// Activate user controller
  /// </summary>
    [Route("api/[controller]")]
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
        /// <response code="400">If any handled exception occurs</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>Returns bool value</returns>
        [HttpPost]
        [Produces(typeof(UserResponseModel))]
        [SwaggerRequestExample(typeof(ActivateUserRequest), typeof(ActivateUserExample))]
        public async Task<ActionResult<UserResponseModel>> ActivateUserAsync(ActivateUserRequest request)
        {
            var activate = await this.mediator.Send(request);
            return new UserResponseModel { Success = activate };
        }
    }
}