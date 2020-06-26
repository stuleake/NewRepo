using Api.PP2.Core.Commands.Forms;
using Api.PP2.Examples;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;

namespace Api.PP2.Controllers
{
    /// <summary>
    /// QuestionSetResponse Api to handle operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionSetResponseController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionSetResponseController"/> class
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public QuestionSetResponseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Submits the response of the Question Set into Planning Portal DB
        /// </summary>
        /// <param name="data"> Response object of Question set</param>
        /// <param name="country">Country value from request header.</param>
        /// <returns>Returns the Question Set defination Id </returns>
        /// <response code="201">If successfully submited the Question Responses</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("submit")]
        [Produces(typeof(Guid))]
        [SwaggerRequestExample(typeof(SubmitQuestionSetResponse), typeof(SubmitQuestionSetResponseExample))]
        public async Task<ActionResult<Guid>> CreateQuestionSetResponseAsync(
            SubmitQuestionSetResponse data,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            data.Country = country;
            data.UserId = CurrentUser;
            var response = await mediator.Send(data);

            return CreatedAtAction(nameof(CreateQuestionSetResponseAsync), response);
        }
    }
}