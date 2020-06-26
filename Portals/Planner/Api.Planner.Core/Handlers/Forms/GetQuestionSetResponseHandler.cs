using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.Services.FormEngine;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Core.Models;

namespace Api.Planner.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Get Question set response
    /// </summary>
    public class GetQuestionSetResponseHandler : IRequestHandler<GetQuestionSetResponse, QuestionSetResponse>
    {
        private readonly IFormEngineClient frmEngineClient;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQuestionSetResponseHandler"/> class.
        /// </summary>
        /// <param name="frmEngineClient">Form Engine client</param>
        /// <param name="configuration">Configuration Data</param>
        public GetQuestionSetResponseHandler(IFormEngineClient frmEngineClient, IConfiguration configuration)
        {
            this.frmEngineClient = frmEngineClient;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<QuestionSetResponse> Handle(GetQuestionSetResponse request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ServiceResponse<QuestionSetResponse> response;

            try
            {
                response = await frmEngineClient.GetQuestionSetResponseAsync(
                    configuration["ApiUri:FormEngine:GetQuestionSetResponse"],
                    JObject.Parse(JsonConvert.SerializeObject(request)),
                    request.AuthToken,
                    request.Country).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = $"Failed to fetch question set response details for application : {request.QSCollectionId}";
                throw new Exception($"{message}", ex);
            }

            if (response == null)
            {
                return null;
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