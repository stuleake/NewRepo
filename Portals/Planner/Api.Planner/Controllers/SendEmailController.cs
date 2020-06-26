using Api.Planner.Core.Commands.Globals;
using Api.Planner.Example;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;

namespace Api.Planner.Controllers
{
    /// <summary>
    /// SendEmail controller to send email through Globals
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public SendEmailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        ///  A Api to send Email
        /// </summary>
        /// <param name="cmd">object of Email Request</param>
        /// <returns>Returns true if success</returns>
        /// <response code="200">If successfully sent the email</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Produces(typeof(bool))]
        [Route("send")]
        [SwaggerRequestExample(typeof(EmailRequest), typeof(EmailRequestExample))]
        public async Task<ActionResult<bool>> SendEmailAsync(EmailRequest cmd)
        {
            var isEmailsend = await mediator.Send(cmd);
            return isEmailsend;
        }
    }
}