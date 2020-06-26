using Api.Planner.Core.Commands.Forms.QuestionSet;
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
    /// Manage the  Get QuestionSet By Application Type
    /// </summary>
    public class GetQuestionSetByApplicationTypeHandler : IRequestHandler<GetByApplicationType, List<QuestionSet>>
    {
        private readonly IFormEngineClient frmEngineClient;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQuestionSetByApplicationTypeHandler"/> class.
        /// </summary>
        /// <param name="frmEngineClient">Form Engine Client.</param>
        /// <param name="configuration">configuration</param>
        public GetQuestionSetByApplicationTypeHandler(IFormEngineClient frmEngineClient, IConfiguration configuration)
        {
            this.frmEngineClient = frmEngineClient;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<List<QuestionSet>> Handle(GetByApplicationType request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ServiceResponse<List<QuestionSet>> response;

            try
            {
                // looks for the question set of the respective id from the forms engine Db.
                response = await frmEngineClient.GetQuestionSetsByApplicationTypeAsync(
                                string.Format(configuration["ApiUri:FormEngine:GetQuestionSetByApplicationType"], request.QSCollectionTypeId),
                                request.AuthToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = $"Failed to fetch question set details for Application Type Id : {request.QSCollectionTypeId}";
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