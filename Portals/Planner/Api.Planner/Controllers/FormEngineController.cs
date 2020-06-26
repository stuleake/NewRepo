using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Filters;

namespace Api.Planner.Controllers
{
    /// <summary>
    /// Controller to perform operations on forms db
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FormEngineController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormEngineController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediatR being passed using dependency injection</param>
        public FormEngineController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// GET all the Taxonomies for the given Application Type
        /// </summary>
        /// <param name="cmd">Application number and language input</param>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <param name="country">Country request header.</param>
        /// <param name="product">Product request header.</param>
        /// <returns>Returns all taxonomies</returns>
        /// <response code="200">If successfully returned the list of taxonomies</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("applicationtype/taxonomies")]
        [CustomAuthorize(
            RoleTypes.TQSuperAdmin,
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<List<QuestionSetWithTaxonomies>>> GetTaxonomiesByApplicationTypeAsync(
            TaxonomiesByApplicationType cmd,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country,
            [FromHeader(Name = RequestHeaderConstants.Product)]string product)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            cmd.AuthToken = authorizationToken;
            cmd.Product = product;
            cmd.Country = country;
            var response = await mediator.Send(cmd);
            return response;
        }
    }
}