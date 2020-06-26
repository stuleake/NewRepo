using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Filters;

namespace Api.Planner.Controllers
{
    /// <summary>
    /// Application Type Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationTypeController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTypeController"/> class.
        /// </summary>
        /// <param name="mediator">Mediator to resolve dependencies</param>
        public ApplicationTypeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get Application Type
        /// </summary>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <param name="country">Country request header.</param>
        /// <param name="product">Product request header.</param>
        /// <returns>List of Application type</returns>
        [HttpGet]
        [Route("all")]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        [Produces(typeof(List<ApplicationTypeModel>))]
        public async Task<ActionResult<List<ApplicationTypeModel>>> GetApplicationTypeAsync(
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country,
            [FromHeader(Name = RequestHeaderConstants.Product)]string product)
        {
            var cmd = new GetApplicationType
            {
                Country = country,
                AuthToken = authorizationToken,
                Product = product
            };
            var response = await mediator.Send(cmd).ConfigureAwait(false);
            return response;
        }
    }
}