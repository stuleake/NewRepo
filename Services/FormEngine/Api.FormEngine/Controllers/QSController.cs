using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Commands.Forms.QuestionSet;
using Api.FormEngine.Core.ViewModels;
using Api.FormEngine.Core.ViewModels.QuestionSets;
using Api.FormEngine.Examples;
using Api.FormEngine.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Filters;

namespace Api.FormEngine.Controllers
{
    /// <summary>
    /// Question Set Api to Perform Question Set related operation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QSController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="QSController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public QSController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns the Form Definition for the form based on the Form Id
        /// </summary>
        /// <param name="id">Form Definition Id</param>
        /// <returns>Returns the Question set for given Id</returns>
        /// <response code="200">If successfully returned the Question Definition</response>
        /// <response code="204">If No Form definition found with the Id</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpGet]
        [Route("{id}")]
        [Produces(typeof(QuestionSetDetails))]
        [SwaggerRequestExample(typeof(Guid), typeof(GuidExample))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<QuestionSetDetails>> GetQuestionSetAsync(Guid id)
        {
            var cmd = new GetQuestionSetRequest { Id = id };

            var jsonSchema = await mediator.Send(cmd);
            return jsonSchema;
        }

        /// <summary>
        /// GET all the Question Sets for the Application Type
        /// </summary>
        /// <param name="id">Application Type Id</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">If successfully returned the Question Definition</response>
        /// <response code="204">If No Form definition found with the Id</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpGet]
        [Route("applicationtype/{id}")]
        [Produces(typeof(QuestionSet))]
        [SwaggerRequestExample(typeof(Guid), typeof(GuidExample))]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<List<QuestionSet>>> GetApplicationTypeAsync(Guid id)
        {
            var cmd = new GetByApplicationType { QSCollectionTypeId = id };

            var jsonSchema = await mediator.Send(cmd);
            return jsonSchema;
        }

        /// <summary>
        ///  Validate Excel File
        /// </summary>
        /// <param name="cmd">Uploaded file content</param>
        /// <param name="product">Product request header value.</param>
        /// <param name="authorizationToken">Authorization token header value.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">If successfully returned the Question Definition</response>
        /// <response code="204">If No Form definition found with the Id</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("Upload")]
        [Produces(typeof(string))]
        [CustomAuthorize(RoleTypes.PP2SuperAdmin)]
        public async Task<ActionResult<string>> UploadQuestionSetAsync(
            [FromForm]ValidateQuestionSetFile cmd,
            [FromHeader(Name = RequestHeaderConstants.Product)]string product,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            cmd.AuthToken = authorizationToken;
            cmd.Product = product;
            cmd.UserId = CurrentUser;
            var response = await mediator.Send(cmd);
            return CreatedAtAction(nameof(UploadQuestionSetAsync), response);
        }

        /// <summary>
        /// Download csv from db
        /// </summary>
        /// <param name="cmd">QsNo and QsVersion input</param>
        /// <param name="authorizationToken">Authorization token header value.</param>
        /// <returns>Returns csv string</returns>
        [HttpPost]
        [Route("DownloadCsv")]
        [CustomAuthorize(RoleTypes.PP2SuperAdmin, RoleTypes.TQSuperAdmin)]
        public async Task<ActionResult<TaxonomyDownload>> DownloadTaxonomyCsvAsync(
            DownloadTaxonomyCsv cmd,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            cmd.AuthToken = authorizationToken;
            var response = await mediator.Send(cmd);
            return response;
        }

        /// <summary>
        /// Api to upload taxonomy csv
        /// </summary>
        /// <param name="cmd">An object that contains the file content</param>
        /// <param name="authorizationToken">Authorization token header value.</param>
        /// <returns>Returns the response for the taxonomy upload operation</returns>
        [HttpPost]
        [Route("uploadtaxonomy")]
        [Produces(typeof(TaxonomyUploadResponse))]
        [CustomAuthorize(RoleTypes.PP2SuperAdmin)]
        public async Task<ActionResult<string>> CreateTaxonomyAsync(
            [FromForm]UploadTaxonomy cmd,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            cmd.AuthToken = authorizationToken;
            var response = await mediator.Send(cmd);
            return CreatedAtAction(nameof(CreateTaxonomyAsync), response);
        }

        /// <summary>
        /// GET all the Taxonomies for the given Application Type
        /// </summary>
        /// <param name="cmd">Application number and language input</param>
        /// <param name="authorizationToken">Authorization token header value.</param>
        /// <param name="product">Product header value.</param>
        /// <param name="country">Country header value.</param>
        /// <returns>Returns all taxonomies</returns>
        [HttpPost]
        [Route("applicationtype/taxonomies")]
        [CustomAuthorize(
            RoleTypes.TQSuperAdmin,
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<List<QuestionSetWithTaxonomies>>> GetTaxonomiesByApplicationTypeAsync(
            GetTaxonomiesByApplicationType cmd,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Product)]string product,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            cmd.AuthToken = authorizationToken;
            cmd.Product = product;
            cmd.Country = country;
            var response = await mediator.Send(cmd);
            return response;
        }
    }
}