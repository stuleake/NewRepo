using Api.PP2.Core.Commands.SubmittedApplications;
using Api.PP2.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;

namespace Api.PP2.Controllers
{
    /// <summary>
    /// Api to list Submitted applications for a user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubmittedApplicationListController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmittedApplicationListController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public SubmittedApplicationListController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// post method to list submitted application of a user
        /// </summary>
        /// <param name="country">Country value from request header.</param>
        /// <param name="portal">Portal value from request header.</param>
        /// <returns>A <see cref="Task{ApplicationListModel}"/> representing the result of the asynchronous operation</returns>
        [HttpGet]
        [Route("getapps")]
        [Produces(typeof(ApplicationListModel))]
        public async Task<ActionResult<ApplicationListModel>> GetSubmittedApplicationsAsync(
            [FromHeader(Name = RequestHeaderConstants.Country)]string country,
            [FromHeader(Name = RequestHeaderConstants.Portal)]string portal)
        {
            var cmd = new SubmittedApplicationsRequestModel
            {
                Role = this.Roles,
                Country = country,
                UserId = this.CurrentUser,
                Portal = portal
            };
            var response = await mediator.Send(cmd);
            return response;
        }
    }
}