using Api.FormEngine.Core.Commands.Forms.Temp;
using Api.FormEngine.Core.ViewModels.Temp;
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
    /// QuestionSetResponse Api to handle FormEngine operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionSetResponseTempController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionSetResponseTempController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public QuestionSetResponseTempController(IMediator mediator) => this.mediator = mediator;

        /// <summary>
        /// Submits the response of the Question Set into Session DB.
        /// </summary>
        /// <param name="data">Object that contains the Application Id, Application Name</param>
        /// <param name="authorizationToken">Authorization token request header value.</param>
        /// <param name="country">Country request header value.</param>
        /// <returns>Returns the Question Set Submit Response with Response code and error messages</returns>
        /// <response code="400">If there are validation errors.</response>
        /// <response code="201">If successfully submited the Question Responses</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("submit")]
        [Produces(typeof(QuestionSetSubmit))]
        public async Task<ActionResult<bool>> SubmitResponse(
            SubmitQuestionSetResponse data,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            data.AuthToken = authorizationToken;
            data.Country = country;
            data.UserId = this.CurrentUser;
            var response = await mediator.Send(data);

            return CreatedAtAction(nameof(SubmitResponse), response);
        }
    }
}