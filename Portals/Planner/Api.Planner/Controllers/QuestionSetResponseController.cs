using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Filters;

namespace Api.Planner.Controllers
{
    /// <summary>
    /// QuestionSetResponse Api to handle FormEngine operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionSetResponseController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionSetResponseController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public QuestionSetResponseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns the Question set response based on the Question Set Id
        /// </summary>
        /// <param name="command">Instance of GetQuestionSetResponse.</param>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <returns>Returns an instance of QuestionSetResponse.</returns>
        /// <response code="200">If successfully returned the form data from session DB</response>
        /// <response code="204">If no data found for the application</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpGet]
        [Route("fetch")]
        [Produces(typeof(QuestionSetResponse))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<QuestionSetResponse>> GetQuestionSetResponseAsync(
            [FromQuery]GetQuestionSetResponse command,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.AuthToken = authorizationToken;
            var jsonSchema = await mediator.Send(command);
            return Ok(jsonSchema);
        }

        /// <summary>
        /// Saves the response of the Question Set into Session DB.
        /// </summary>
        /// <param name="command">Response object of Question set</param>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <param name="country">Country request header.</param>
        /// <returns>Returns the Question Set definition Id </returns>
        /// <response code="201">If Question Set Response saved successfully to DB</response>
        /// <response code="400">If Question Set Response have invalid data</response>
        /// <response code="500">If any unhandled error occurs</response>
        [HttpPost]
        [Route("save")]
        [Produces(typeof(Guid))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<Guid>> CreateQuestionSetResponseAsync(
            CreateQuestionSetResponse command,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.AuthToken = authorizationToken;
            command.Country = country;
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(CreateQuestionSetResponseAsync), response);
        }

        /// <summary>
        /// Validates Question set response.
        /// </summary>
        /// <param name="data">Object that contains the Application Id, Application Name</param>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <param name="country">Country request header.</param>
        /// <returns>Returns the Question Set Submit Response with Response code and error messages</returns>
        /// <response code="400">If there are validation errors.</response>
        /// <response code="201">If successfully submited the Question Responses</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("submit")]
        [Produces(typeof(QuestionSetSubmit))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<bool>> ValidateQuestionSetResponseAsync(
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
            var response = await mediator.Send(data);

            return CreatedAtAction(nameof(CreateQuestionSetResponseAsync), response);
        }
    }
}