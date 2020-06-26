using Api.FormEngine.Core.Commands.DraftApplications;
using Api.FormEngine.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;

namespace Api.FormEngine.Controllers
{
    /// <summary>
    /// Api to list Drafted application
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DraftApplicationListController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DraftApplicationListController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediatR being passed using dependency injection</param>
        public DraftApplicationListController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Post method to list draft application of a user.
        /// </summary>
        /// <param name="request">request object containing user data.</param>
        /// <param name="portal">Portal request header.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [Route("getapps")]
        [Produces(typeof(ApplicationTypeModel))]
        public async Task<ActionResult<ApplicationListModel>> GetDraftApplicationsAsync(
            DraftApplicationListRequestModel request,
            [FromHeader(Name = RequestHeaderConstants.Portal)]string portal)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            request.Role = this.Roles;
            request.Portal = portal;
            var response = await mediator.Send(request);
            return response;
        }
    }
}