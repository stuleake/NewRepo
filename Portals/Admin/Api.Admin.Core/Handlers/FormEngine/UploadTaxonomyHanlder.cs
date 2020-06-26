using Api.Admin.Core.Commands.FormEngine;
using Api.Admin.Core.Services.FormEngine;
using Api.Admin.Core.ViewModels;
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
    /// Handler to upload taxonomy csv
    /// </summary>
    public class UploadTaxonomyHanlder : IRequestHandler<UploadTaxonomy, TaxonomyUploadResponse>
    {
        private readonly IConfiguration configuration;
        private readonly IFormEngineClient formsEngineClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadTaxonomyHanlder"/> class.
        /// </summary>
        /// <param name="configuration">object of configuration being passed using dependency injection</param>
        /// <param name="formEngineClient">object of formEngineClient being passed using dependency injection</param>
        public UploadTaxonomyHanlder(IConfiguration configuration, IFormEngineClient formEngineClient)
        {
            this.configuration = configuration;
            this.formsEngineClient = formEngineClient;
        }

        /// <inheritdoc/>
        public async Task<TaxonomyUploadResponse> Handle(UploadTaxonomy request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = await formsEngineClient.UploadTaxonomyFileAsync(
                configuration["ApiUri:FormEngine:UploadTaxonomy"],
                request.FileContent,
                request.AuthToken,
                request.Product).ConfigureAwait(false);

            if (response == null)
            {
                throw new ServiceException($"Null response received from the forms engine");
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