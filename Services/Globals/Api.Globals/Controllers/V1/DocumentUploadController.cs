using Api.Globals.Core.Commands.DocumentUpload;
using Api.Globals.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Globals.Controllers.V1
{
    /// <summary>
    /// Api to handle document upload
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]/v1/")]
    [ApiController]
    [Authorize]
    public class DocumentUploadController : ControllerBase
    {
        private readonly IMediator mediator;
        private const long MaxRequestSizLimit = 500 * 1000 * 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentUploadController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediatR being passed using dependency injection</param>
        public DocumentUploadController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Uploads document to the azure blob
        /// </summary>
        /// <param name="request">request object containing document data</param>
        /// <returns>Returns boolean based on the upload status.</returns>
        [HttpPost]
        [Route("DocumentUploadAsync")]
        [RequestSizeLimit(MaxRequestSizLimit)]
        [Produces(typeof(Dictionary<string, string>))]
        public async Task<ActionResult<Dictionary<string, string>>> DocumentUploadAsync([FromForm]DocumentUploadRequestModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var metadata = new List<Dictionary<string, string>>();
            if (request.Metadatas != null)
            {
                metadata = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(request.Metadatas);
            }
            var uploadRequest = new DocumentUploadRequest
            {
                ContainerName = request.ContainerName,
                SubContainerName = request.SubContainerName,
                Documents = request.Documents,
                Metadatas = metadata
            };
            var result = await mediator.Send(uploadRequest);
            return result;
        }
    }
}