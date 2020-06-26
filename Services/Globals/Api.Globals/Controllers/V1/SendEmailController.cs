using Api.Globals.Core.Commands.Email;
using Api.Globals.Examples;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.Globals.Controllers.V1
{
    /// <summary>
    /// Send Email controller
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]/v1/")]
    [ApiController]
    public class SendEmailController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailController"/> class
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public SendEmailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Send Email controller to get Email response
        /// </summary>
        /// <param name="request">object of GetMail Request containing EmailId, EmailType and Name</param>
        /// <response code="200">If successfully returned the Email sent response</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <returns>Returns bool value to get Email is sent or not</returns>
        [HttpPost]
        [Route("SendEmailAsync")]
        [Produces(typeof(bool))]
        [SwaggerRequestExample(typeof(GetEmailRequest), typeof(GetEmailRequestExample))]
        public async Task<ActionResult<bool>> SendEmailAsync(GetEmailRequest request)
        {
            var isMailSent = await this.mediator.Send(request);
            return isMailSent;
        }
    }
}