using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.HttpModel;
using Api.Planner.Core.Services.FormEngine;
using Api.Planner.Core.Services.PP2;
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

namespace Api.Planner.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Submit question set response
    /// </summary>
    public class SubmitQuestionSetResponseHandler : IRequestHandler<SubmitQuestionSetResponse, QuestionSetSubmit>
    {
        private readonly IFormEngineClient formEngineClient;
        private readonly IPP2HttpClient pp2HttpClient;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitQuestionSetResponseHandler"/> class.
        /// </summary>
        /// <param name="formEngineClient">Form Engine Client</param>
        /// <param name="configuration">Configuration data</param>
        /// <param name="pp2HttpClient"> object of PP2Httpclient</param>
        public SubmitQuestionSetResponseHandler(IFormEngineClient formEngineClient, IConfiguration configuration, IPP2HttpClient pp2HttpClient)
        {
            this.formEngineClient = formEngineClient;
            this.configuration = configuration;
            this.pp2HttpClient = pp2HttpClient;
        }

        /// <inheritdoc/>
        public async Task<QuestionSetSubmit> Handle(SubmitQuestionSetResponse request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                // Validate the form.
                var response = await formEngineClient.ValidateQuestionSetResponseAsync(
                     configuration["ApiUri:FormEngine:ValidateQuestionSetResponse"],
                     JObject.Parse(JsonConvert.SerializeObject(request)),
                     request.AuthToken,
                     request.Country).ConfigureAwait(false);

                if (response == null)
                {
                    throw new ServiceException($"Null response received from the forms engine");
                }

                if (response.Code == (int)HttpStatusCode.BadRequest)
                {
                    throw new TQException(response.Message);
                }

                if (response.Code != (int)HttpStatusCode.OK)
                {
                    throw new ServiceException($"Erroneous response received from the PP2 while validating the response");
                }

                if (!response.Value.Status)
                {
                    return response.Value;
                }

                var qsrResponse = await formEngineClient.GetQuestionSetResponseAsync(
                       configuration["ApiUri:FormEngine:GetQuestionSetResponse"],
                       JObject.Parse(JsonConvert.SerializeObject(request)),
                       request.AuthToken,
                       request.Country).ConfigureAwait(false);

                if (qsrResponse.Code != (int)HttpStatusCode.OK)
                {
                    throw new ServiceException($"Some error occurred while fetching data from formsEngine DB. Error Code {qsrResponse.Code} , Error Message {qsrResponse.Message}");
                }

                var model = new QuestionSetResponseModel
                {
                    Response = qsrResponse.Value.Response,
                    QuestionSetId = qsrResponse.Value.QuestionSetId,
                    ApplicationId = qsrResponse.Value.UserApplicationId,
                    ApplicationName = qsrResponse.Value.ApplicationName
                };

                // Submit the form
                var resp = await pp2HttpClient.CreateQuestionSetResponseAsync(
                                    configuration["ApiUri:PP2:SubmitQuestionSetResponse"],
                                    JObject.Parse(JsonConvert.SerializeObject(model)),
                                    request.AuthToken,
                                    request.Country).ConfigureAwait(false);

                if (resp.Code != (int)HttpStatusCode.Created)
                {
                    throw new ServiceException($"Some error occurred while updating the LOB DB. Error Code {resp.Code} , Error Message {resp.Message}");
                }

                var deleteResp = await formEngineClient.DeleteQuestionSetResponseAsync(
                    string.Format(configuration["ApiUri:FormEngine:DeleteQuestionSetResponse"], request.QSCollectionId), request.AuthToken).ConfigureAwait(false);

                if (!deleteResp.Value)
                {
                    throw new ServiceException($"Erroneous response received from the Form Engine while deleting from FormEngines DB");
                }

                response.Value.Status = deleteResp.Value;

                return response.Value;
            }
            catch (Exception ex)
            {
                var message = $"Failed to submit question set response details for application : {request.QSCollectionId}";
                throw new Exception($"{message}", ex);
            }
        }
    }
}