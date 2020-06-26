using Api.Admin.Core.Commands.Blob;
using Api.Admin.Examples;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Admin.Controllers
{
    /// <summary>
    /// Blob Api
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlobController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public BlobController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get a sas token for the container and the file specified.
        /// </summary>
        /// <param name="request">object of GetSasToken</param>
        /// <returns>The Sas Token as string</returns>
        /// <response code="200">If successfully generated the SAS Token</response>
        /// <response code="204">If container Not found</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpGet]
        [Route("sastoken")]
        [Produces(typeof(string))]
        [SwaggerRequestExample(typeof(GetSasToken), typeof(GetSasTokenExample))]
        public async Task<ActionResult<string>> GetSasTokenAsync([FromQuery]GetSasToken request)
        {
            var token = await mediator.Send(request);
            return token;
        }
    }
}