using Api.Admin.Core.Commands.FormEngine;
using Api.Admin.Core.Services.FormEngine;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;

namespace Api.Admin.Core.Handlers.FormEngine
{
    /// <summary>
    /// Handler class to import and save form structure file
    /// </summary>
    public class UploadFormStructureFileHandler : IRequestHandler<UploadFormStructureFile, string>
    {
        private readonly IConfiguration configuration;
        private readonly IFormEngineClient formEngineClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadFormStructureFileHandler"/> class.
        /// </summary>
        /// <param name="configuration">Configuration data</param>
        /// <param name="formEngineClient">form engine client</param>
        public UploadFormStructureFileHandler(IConfiguration configuration, IFormEngineClient formEngineClient)
        {
            this.configuration = configuration;
            this.formEngineClient = formEngineClient;
        }

        /// <inheritdoc/>
        public async Task<string> Handle(UploadFormStructureFile request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = await formEngineClient.UploadFormStructureFileAsync(
                        configuration["ApiUri:FormEngine:FormStructureFileUpload"],
                        request.FileContent,
                        request.AuthToken,
                        request.Product).ConfigureAwait(false);

            if (response == null)
            {
                throw new ServiceException("Null response received from the form engine");
            }

            return response.Code switch
            {
                (int)HttpStatusCode.Created => response.Value,
                (int)HttpStatusCode.BadRequest => throw new TQException(response.Message),
                _ => throw new ServiceException($"Erroneous response received from the forms engine ==> {response.Code} - {response.Message}"),
            };
        }
    }
}