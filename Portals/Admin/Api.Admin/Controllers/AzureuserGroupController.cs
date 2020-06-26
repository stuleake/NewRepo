using Api.Admin.Core.Commands.AzureUser;
using Api.Admin.Examples;
using Api.Admin.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;

namespace Api.Admin.Controllers
{
    /// <summary>
    /// Azure user Group Controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AzureUserGroupController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureUserGroupController"/> class.
        /// </summary>
        /// <param name="mediator">mediator</param>
        public AzureUserGroupController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Gets the Users groups.
        /// </summary>
        /// <param name="objectId">objectid</param>
        /// <response code="200">If successfully user is created</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>list of groups</returns>
        [HttpGet]
        [Route("Groups")]
        [Produces(typeof(Claims))]
        [SwaggerRequestExample(typeof(string), typeof(AzureUserGroupExample))]
        public async Task<IActionResult> GetGroupsAsync(string objectId)
        {
            AzureUserGroupHtmlUrl request = new AzureUserGroupHtmlUrl
            {
                ObjectId = objectId
            };
            var groups = await this.mediator.Send(request);
            Claims claims = new Claims
            {
                Groups = groups
            };
            return Ok(claims);
        }

        /// <summary>
        /// Create group in Azure
        /// </summary>
        /// <param name="command">Group Details</param>
        /// <response code="200">If successfully user is created</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>bool</returns>
        [HttpPost]
        [Route("CreateGroup")]
        [Produces(typeof(bool))]
        public async Task<ActionResult<bool>> CreateGroupAsync(AzureUserGroupModel command)
        {
            var isCreated = await mediator.Send(command);
            return isCreated;
        }

        /// <summary>
        /// Assign User to Group in Azure
        /// </summary>
        /// <param name="command">Group and User object id</param>
        /// <response code="200">If successfully user is created</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>bool</returns>
        [HttpPost]
        [Route("AssignUsertoGroup")]
        [Produces(typeof(bool))]
        public async Task<ActionResult<bool>> AssignUserToGroupAsync(AssignUsertoGroupModel command)
        {
            var isCreated = await mediator.Send(command);
            return isCreated;
        }
    }
}