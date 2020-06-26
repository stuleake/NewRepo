using Api.Planner.Core.Commands.Forms.QuestionSet;
using Api.Planner.Core.Services.FormEngine;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Core.Models;

namespace Api.Planner.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Get question set
    /// </summary>
    public class GetQuestionSetHandler : IRequestHandler<GetQuestionSet, QuestionSet>
    {
        private readonly IFormEngineClient frmEngineClient;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQuestionSetHandler"/> class.
        /// </summary>
        /// <param name="frmEngineClient">object of form engine client</param>
        /// <param name="configuration">configuration object</param>
        public GetQuestionSetHandler(IFormEngineClient frmEngineClient, IConfiguration configuration)
        {
            this.frmEngineClient = frmEngineClient;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<QuestionSet> Handle(GetQuestionSet request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ServiceResponse<QuestionSet> response;

            try
            {
                // looks for the question set of the respective id from the forms engine Db.
                response = await frmEngineClient.GetQuestionSetAsync(
                    $"{configuration["ApiUri:FormEngine:GetQuestionSet"]}{request.Id}",
                    request.AuthToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = $"Failed to fetch question set details for question set : {request.Id}";
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