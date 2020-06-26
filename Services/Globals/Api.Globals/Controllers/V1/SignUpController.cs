using Api.Globals.Core.Commands.SignUp;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Globals.Controllers.V1
{
    /// <summary>
    /// Register controller
    /// </summary>
    [ApiVersion("1.0")]
    //[ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]/v{version:apiVersion}")]
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
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>Returns bool value</returns>
        [HttpPost]
        [Route("CreateUserSignUpAsync")]
        [Produces(typeof(bool))]
        public async Task<ActionResult<bool>> CreateUserSignUpAsync(SignUpRequest request)
        {
            var register = await this.mediator.Send(request);
            return register;
        }

        /// <summary>
        /// Deletes AzureUser
        /// </summary>
        /// <param name="request">Email of user</param>
        /// <response code="200">If successfully returned the Email sent response</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>Returns bool value to get Email is sent or not</returns>
        [HttpDelete]
        [Route("deleteuser")]
        //[MapToApiVersion("1")]
        [Produces(typeof(bool))]
        public async Task<ActionResult<bool>> DeleteUser(DeleteUserRequest request)
        {
            var userDeleted = await this.mediator.Send(request);
            return userDeleted;
        }
    }
}