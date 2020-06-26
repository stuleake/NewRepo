using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.ViewModels;
using Api.FormEngine.Examples;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Filters;

namespace Api.FormEngine.Controllers
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
        /// Initializes a new instance of the <see cref="QuestionSetResponseController"/> class
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public QuestionSetResponseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns the Form Data for the form based on the Question Set Definition Id
        /// </summary>
        /// <param name="command">Form Definition Id</param>
        /// <param name="country">Country request header value.</param>
        /// <returns>Returns the Form Data for given Question Set ID</returns>
        /// <response code="200">If successfully returned the Question Set Response</response>
        /// <response code="204">If no application found with the name</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("fetch")]
        [Produces(typeof(QuestionSetResponse))]
        [SwaggerRequestExample(typeof(GetQuestionSetResponse), typeof(GetQuestionSetResponseExample))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<QuestionSetResponse>> GetQuestionSetResponseAsync(
            GetQuestionSetResponse command,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.UserId = CurrentUser;
            command.Country = country;
            var jsonSchema = await mediator.Send(command);
            return jsonSchema;
        }

        /// <summary>
        /// Saves the response of the Question Set into Session DB
        /// </summary>
        /// <param name="data">Response object of Question set</param>
        /// <param name="country">Country request header value.</param>
        /// <returns>Returns the Question Set defination Id </returns>
        /// <response code="201">If Question Set Response saved successfully to DB</response>
        /// <response code="400">If Question Set Response have invalid data</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("save")]
        [Produces(typeof(Guid))]
        [SwaggerRequestExample(typeof(CreateQuestionSetResponse), typeof(CreateQuestionSetResponseExample))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<Guid>> CreateQuestionSetResponseAsync(
            CreateQuestionSetResponse data,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            data.UserId = CurrentUser;
            data.Country = country;
            var response = await mediator.Send(data);
            return CreatedAtAction(nameof(CreateQuestionSetResponseAsync), response);
        }

        /// <summary>
        /// Submits the response of the Question Set into Session DB
        /// </summary>
        /// <param name="data">Object that contains the QuestionSetId and Application Name</param>
        /// <param name="country">Country request header value.</param>
        /// <returns>Returns the Question Set Validate Response</returns>
        /// <response code="200">If Api is successful but there are validation errors</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("validate")]
        [Produces(typeof(QuestionSetValidate))]
        [SwaggerRequestExample(typeof(ValidateQuestionSetResponse), typeof(ValidateQuestionSetResponseExample))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<QuestionSetValidate>> ValidateQuestionSetResponseAsync(
            ValidateQuestionSetResponse data,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            data.UserId = CurrentUser;
            data.Country = country;
            var response = await mediator.Send(data);

            return Ok(response);
        }

        /// <summary>
        /// Delete the question set from Session DB
        /// </summary>
        /// <param name="id">Form Definition Id</param>
        /// <returns>Returns the Question Set Delete Response</returns>
        /// <response code="200">If Api is successful but there are validation errors</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpDelete]
        [Route("{id}")]
        [Produces(typeof(bool))]
        [SwaggerRequestExample(typeof(Guid), typeof(GuidExample))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<bool>> DeleteQuestionSetResponseAsync(Guid id)
        {
            DeleteQuestionSetResponse cmd = new DeleteQuestionSetResponse { Id = id };
            var response = await mediator.Send(cmd);
            return Ok(response);
        }
    }
}