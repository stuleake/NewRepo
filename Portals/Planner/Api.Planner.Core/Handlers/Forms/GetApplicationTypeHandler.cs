using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.Services.FormEngine;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Core.Models;

namespace Api.Planner.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to get Application Types
    /// </summary>
    public class GetApplicationTypeHandler : IRequestHandler<GetApplicationType, List<ApplicationTypeModel>>
    {
        private readonly IFormEngineClient frmEngineClient;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetApplicationTypeHandler"/> class.
        /// </summary>
        /// <param name="frmEngineClient">Form engine client object</param>
        /// <param name="configuration">Configuration object</param>
        public GetApplicationTypeHandler(IFormEngineClient frmEngineClient, IConfiguration configuration)
        {
            this.frmEngineClient = frmEngineClient;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<List<ApplicationTypeModel>> Handle(GetApplicationType request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ServiceResponse<List<ApplicationTypeModel>> response;

            try
            {
                response = await frmEngineClient.GetApplicationTypesAsync(
                    configuration["ApiUri:FormEngine:ListApplicationTypes"],
                    request.AuthToken,
                    request.Country,
                    request.Product).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = $"Failed to fetch Application Type : {request.Country}";
                throw new Exception($"{message}", ex);
            }

            if (response == null)
            {
                throw new ServiceException($"Null response received from the forms engine");
            }

            return response.Code switch
            {
                (int)HttpStatusCode.OK => response.Value,
                (int)HttpStatusCode.NoContent => null,
                _ => throw new ServiceException($"Erroneous response received from the forms engine ==> {response.Code} - {response.Message}"),
            };
        }
    }
}