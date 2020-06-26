using Api.Planner.Core.Commands.Gatsby;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Planner.Controllers
{
    /// <summary>
    /// GatsbyBuild Controller to handle Gatsby call
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GatsbyBuildController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GatsbyBuildController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public GatsbyBuildController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns boolean on successful calling the Gatsby builder
        /// </summary>
        /// <param name="id">Gatsby id</param>
        /// <returns>Return true on successfully calling the Gatsby builder</returns>
        /// <response code="200">If successfully returned </response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpGet]
        [Route("{id}")]
        [Produces(typeof(bool))]
        public async Task<ActionResult<bool>> GetCallGatsbyBuilderAsync(int id)
        {
            var definition = new Definition { Id = id };
            var cmd = new GatsbyDefinition { Definition = definition };
            var isbuilderStarted = await mediator.Send(cmd);
            return isbuilderStarted;
        }
    }
}