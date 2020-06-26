using Api.Admin.Core.Commands.FormEngine;
using Api.Admin.Core.Services.FormEngine;
using Api.Admin.Core.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;

namespace Api.Admin.Core.Handlers.FormEngine
{
    /// <summary>
    /// Handler class to download taxonomy as csv
    /// </summary>
    public class TaxonomyFileDownloadHandler : IRequestHandler<TaxonomyFileDownload, DownloadCsv>
    {
        private readonly IConfiguration configuration;
        private readonly IFormEngineClient formEngineClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonomyFileDownloadHandler"/> class.
        /// </summary>
        /// <param name="configuration">Configuration data</param>
        /// <param name="formEngineClient">form engine client</param>
        public TaxonomyFileDownloadHandler(IConfiguration configuration, IFormEngineClient formEngineClient)
        {
            this.configuration = configuration;
            this.formEngineClient = formEngineClient;
        }

        /// <inheritdoc/>
        public async Task<DownloadCsv> Handle(TaxonomyFileDownload request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = await formEngineClient.TaxonomyFileDownloadAsync(
                        configuration["ApiUri:FormEngine:TaxonomyCsvDownload"],
                        JObject.Parse(JsonConvert.SerializeObject(request)),
                        request.AuthToken,
                        request.Product).ConfigureAwait(false);

            if (response == null)
            {
                throw new ServiceException("Null response received from the form engine");
            }

            return response.Code switch
            {
                (int)HttpStatusCode.OK => response.Value,
                (int)HttpStatusCode.BadRequest => throw new TQException(response.Message),
                _ => throw new ServiceException($"Erroneous response received from the forms engine ==> {response.Code} - {response.Message}"),
            };
        }
    }
}