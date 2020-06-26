using Api.Planner.Core.Commands.Forms.QuestionSet;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Filters;

namespace Api.Planner.Controllers
{
    /// <summary>
    /// Question Set Api to Perform Question Set related operation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionSetController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionSetController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public QuestionSetController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns the Question set based on QuestionSetId.
        /// </summary>
        /// <param name="id">Form Definition Id</param>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <returns>Returns the Question set for given Id</returns>
        /// <response code="200">If successfully returned the json schema(Form Definition)</response>
        /// <response code="500">If any unhandled exception occurs</response>
        /// <response code="204">If No form definition found.</response>
        [HttpGet]
        [Route("{id}")]
        [Produces(typeof(QuestionSet))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<QuestionSet>> GetQuestionSetAsync(
            Guid id,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken)
        {
            GetQuestionSet cmd = new GetQuestionSet { Id = id, AuthToken = authorizationToken };

            var jsonSchema = await mediator.Send(cmd);
            return jsonSchema;
        }

        /// <summary>
        /// Returns the QuestionSet based on Application Id.
        /// </summary>
        /// <param name="id">Application id</param>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <returns>List of QuestionSet</returns>
        [HttpGet]
        [Route("applicationtype/{id}")]
        [Produces(typeof(QuestionSet))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<List<QuestionSet>>> GetQuestionSetByApplicationTypeAsync(
            Guid id,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken)
        {
            GetByApplicationType cmd = new GetByApplicationType { QSCollectionTypeId = id, AuthToken = authorizationToken };

            var jsonSchema = await mediator.Send(cmd);
            return jsonSchema;
        }
    }
}