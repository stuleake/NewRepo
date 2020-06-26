using Api.Planner.Core.Commands.SignUp;
using Api.Planner.Core.ViewModels;
using Api.Planner.Example;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Planner.Controllers
{
    /// <summary>
    /// Register controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpController"/> class
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public SignUpController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Creates azure user
        /// </summary>
        /// <param name="request">object of SignUp Request </param>
        /// <response code="200">If successfully returned the Email sent response</response>
        /// <response code="400">If any handled exception occurs</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>Returns bool value</returns>
        [HttpPost]
        [Produces(typeof(UserResponseModel))]
        [SwaggerRequestExample(typeof(SignUpRequest), typeof(SignUpExample))]
        public async Task<ActionResult<UserResponseModel>> UserSignUpAsync(SignUpRequest request)
        {
            var register = await this.mediator.Send(request);
            return new UserResponseModel { Success = register };
        }
    }
}