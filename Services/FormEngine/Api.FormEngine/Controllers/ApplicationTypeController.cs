using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Filters;

namespace Api.FormEngine.Controllers
{
    /// <summary>
    /// API for the Application Type Controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationTypeController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationTypeController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediatR being passed using dependency injection</param>
        public ApplicationTypeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// GET Method to list all the type of applications
        /// </summary>
        /// <param name="countryHeader">Country request header.</param>
        /// <param name="productHeader">Product request header</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
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
        public async Task<ActionResult<List<ApplicationTypeModel>>> GetApplicationTypesAsync(
            [FromHeader(Name = RequestHeaderConstants.Country)]string countryHeader,
            [FromHeader(Name = RequestHeaderConstants.Product)]string productHeader)
        {
            var cmd = new GetApplicationType
            {
                Country = countryHeader,
                Product = productHeader
            };
            var response = await mediator.Send(cmd);
            return response;
        }
    }
}