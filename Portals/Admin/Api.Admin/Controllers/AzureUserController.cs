using Api.Admin.Core.Commands.AzureUser;
using Api.Admin.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Admin.Controllers
{
    /// <summary>
    /// AzureUser Api to manage Azure users
    /// </summary>
    [ApiController]
    public class AzureUserController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureUserController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public AzureUserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Creates the user in B2C
        /// </summary>
        /// <param name="command">Request to create azure user with all necessary user details</param>
        /// <response code="200">If successfully user is created</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>return bool if user is created.</returns>
        [HttpPost]
        [Route("CreateUser")]
        [Produces(typeof(string))]
        public async Task<bool> CreateUserAsync(AzureUserModel command)
        {
            var token = await mediator.Send(command);
            return token;
        }

        /// <summary>
        /// Get All user from B2C
        /// </summary>
        /// <param name="active">Request 'True' to get all active azure user </param>
        /// <response code="200">If successfully gets the users list</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>return List of user</returns>
        [HttpGet]
        [Route("GetUsers")]
        [Produces(typeof(AzureUserDataViewModel))]
        public async Task<IEnumerable<AzureUserObject>> GetUsersAsync(bool active)
        {
            AzureUserHtmlUrl request = new AzureUserHtmlUrl
            {
                Active = active
            };
            var azureUserList = await mediator.Send(request);
            return azureUserList;
        }
    }
}