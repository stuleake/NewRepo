using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.Services.FormEngine;
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
    /// Handler to Create response of question set
    /// </summary>
    public class CreateQuestionSetResponseHandler : IRequestHandler<CreateQuestionSetResponse, Guid>
    {
        private readonly IFormEngineClient frmHttpClient;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQuestionSetResponseHandler"/> class.
        /// </summary>
        /// <param name="frmHttpClient">object of Form Client</param>
        /// <param name="configuration">configuration data</param>
        public CreateQuestionSetResponseHandler(IFormEngineClient frmHttpClient, IConfiguration configuration)
        {
            this.frmHttpClient = frmHttpClient;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<Guid> Handle(CreateQuestionSetResponse request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ServiceResponse<Guid> response;

            try
            {
                response = await frmHttpClient.CreateQuestionSetResponseAsync(
                    configuration["ApiUri:FormEngine:SaveQuestionSetResponse"],
                    JObject.Parse(JsonConvert.SerializeObject(request)),
                    request.AuthToken,
                    request.Country).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = $"Failed to save question set response for question set : {request.QuestionSetId} and application name : {request.ApplicationName}";
                throw new Exception($"{message}", ex);
            }

            if (response == null)
            {
                throw new ServiceException($"Null response received from the forms engine");
            }

            if (response.Code != (int)HttpStatusCode.Created)
            {
                throw new ServiceException($"Erroneous response received from the forms engine ==> {response.Code} - {response.Message}");
            }

            return response.Value;
        }
    }
}