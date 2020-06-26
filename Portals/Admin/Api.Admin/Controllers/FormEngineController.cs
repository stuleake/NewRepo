using Api.Admin.Core.Commands.FormEngine;
using Api.Admin.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Filters;

namespace Api.Admin.Controllers
{
    /// <summary>
    /// Controller to import fee calculator rules for applications.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FormEngineController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormEngineController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediatR being passed using dependency injection</param>
        public FormEngineController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api to upload form structure file
        /// </summary>
        /// <param name="cmd">An object contain file content</param>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <param name="product">Product request header.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">If successfully returned the config settings</response>
        /// <response code="204">If No Form definition found with the Id</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("upload")]
        [Produces(typeof(string))]
        [CustomAuthorize(RoleTypes.PP2SuperAdmin)]
        public async Task<ActionResult<string>> CreateUploadFormStructureFileAsync(
            [FromForm]UploadFormStructureFile cmd,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Product)]string product)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            cmd.AuthToken = authorizationToken;
            cmd.Product = product;
            var response = await mediator.Send(cmd);

            return CreatedAtAction(nameof(CreateUploadFormStructureFileAsync), response);
        }

        /// <summary>
        /// Download csv from db
        /// </summary>
        /// <param name="cmd">QsNo and QsVersion input</param>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <param name="product">Product request header.</param>
        /// <returns>Returns csv string</returns>
        /// <response code="200">If successfully downloaded the taxonomy CSV</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("DownloadCsv")]
        [CustomAuthorize(RoleTypes.TQSuperAdmin, RoleTypes.PP2SuperAdmin)]
        public async Task<ActionResult<DownloadCsv>> DownloadTaxonomyCsvAsync(
            TaxonomyFileDownload cmd,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Product)]string product)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            cmd.AuthToken = authorizationToken;
            cmd.Product = product;
            var response = await mediator.Send(cmd);
            return response;
        }

        /// <summary>
        /// Api to upload taxonomy csv
        /// </summary>
        /// <param name="cmd">An object that contains the file content</param>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <returns>Returns the response for the taxonomy upload operation</returns>
        /// <response code="200">If successfully uploaded the taxonomy</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpPost]
        [Route("uploadtaxonomy")]
        [Produces(typeof(TaxonomyUploadResponse))]
        [CustomAuthorize(RoleTypes.PP2SuperAdmin)]
        public async Task<ActionResult<TaxonomyUploadResponse>> UploadTaxonomyAsync(
            [FromForm]UploadTaxonomy cmd,
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
    }
}