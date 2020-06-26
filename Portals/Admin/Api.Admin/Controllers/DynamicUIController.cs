using Api.Admin.Core.Commands.DynamicUI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Admin.Controllers
{
    /// <summary>
    /// Dynamic UI Api
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicUIController : BaseApiController
    {
        private readonly IMediator mediator;
        private readonly IWebHostEnvironment env;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicUIController"/> class.
        /// </summary>
        /// <param name="env">Object of hosting Environment being passed using Dependency injection</param>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public DynamicUIController(IWebHostEnvironment env, IMediator mediator)
        {
            this.env = env;
            this.mediator = mediator;
        }

        /// <summary>
        /// Gives the HTML page as per country specified
        /// </summary>
        /// <param name="country">country name</param>
        /// <response code="200">If successfully returned the html</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>Returns the Content text as html</returns>
        [HttpGet]
        [Route("unified")]
        [Produces(typeof(string))]
        public async Task<IActionResult> GetHtmlContentAsync(string country)
        {
            var request = new GetHtmlUrl
            {
                Country = country,
                ContentRootPath = env.ContentRootPath
            };
            var content = await mediator.Send(request);
            return Content(content, "text/html");
        }

        /// <summary>
        /// Gives the HTML page as per country specified
        /// </summary>
        /// <param name="country">country name</param>
        /// <response code="200">If successfully returned the html</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>Returns the Content text as html</returns>
        [HttpGet]
        [Route("ResetPasswordUI")]
        [Produces(typeof(string))]
        public async Task<IActionResult> GetResetPasswordUiAsync(string country)
        {
            var request = new GetResetPasswordHtmlUrl
            {
                Country = country,
            };
            var content = await mediator.Send(request);
            return Content(content, "text/html");
        }

        /// <summary>
        /// Gets the claims for SignIn
        /// </summary>
        /// <returns>returns the permission.</returns>
        [HttpPost]
        [Route("signin")]
        [Produces(typeof(string))]
        public async Task<IActionResult> PostSignInAsync()
        {
            Claims claims = new Claims
            {
                Permission = "read"
            };
            return await Task.FromResult(Ok(claims)).ConfigureAwait(false);
        }

        private class Claims
        {
            /// <summary>
            /// Gets or Sets the user permissions
            /// </summary>
            public string Permission { get; set; }
        }
    }
}