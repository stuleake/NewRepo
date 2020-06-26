using Api.Admin.Core.Commands.ApplicationList;
using Api.Admin.Core.Services.FormEngine;
using Api.Admin.Core.Services.PP2;
using Api.Admin.Core.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Exceptions;
using TQ.Core.Models;

namespace Api.Admin.Core.Handlers.ApplicationList
{
    /// <summary>
    /// A handler to list all the application for a user.
    /// </summary>
    public class ApplicationListHandler : IRequestHandler<ApplicationListRequestModel, ApplicationListModel>
    {
        private readonly IFormEngineClient frmHttpClient;
        private readonly IConfiguration configuration;
        private readonly IPP2HttpClient pp2HttpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationListHandler"/> class.
        /// </summary>
        /// <param name="frmHttpClient">Form Engine Client.</param>
        /// <param name="pp2HttpClient">PP2 client</param>
        /// <param name="configuration">Configuration data.</param>
        public ApplicationListHandler(IFormEngineClient frmHttpClient, IPP2HttpClient pp2HttpClient, IConfiguration configuration)
        {
            this.frmHttpClient = frmHttpClient;
            this.configuration = configuration;
            this.pp2HttpClient = pp2HttpClient;
        }

        /// <inheritdoc/>
        public async Task<ApplicationListModel> Handle(ApplicationListRequestModel request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ServiceResponse<ApplicationListModel> response;

            if (request.Type == ApplicationStatusConstants.Drafts)
            {
                response = await frmHttpClient.GetDraftApplicationListAsync(
                    configuration["ApiUri:FormEngine:DraftApplicationListResponse"],
                    JObject.Parse(JsonConvert.SerializeObject(request)),
                    request.AuthToken,
                    "Admin").ConfigureAwait(false);
            }
            else if (request.Type == ApplicationStatusConstants.Submitted)
            {
                response = await pp2HttpClient.GetSubmittedApplicationsAsync(
                    configuration["ApiUri:PP2:SubmittedApplicationListResponse"],
                    request.AuthToken,
                    request.Country,
                    "Admin").ConfigureAwait(false);
            }
            else
            {
                throw new TQException($"Invalid application type {request.Type} in request.");
            }

            if (response == null)
            {
                if (request.Type == ApplicationStatusConstants.Drafts)
                {
                    throw new ServiceException("Null response received from the forms engine");
                }

                throw new ServiceException("Null response received from the http client");
            }

            if (response.Code != (int)HttpStatusCode.OK)
            {
                throw new ServiceException($"Erroneous response received from the forms engine ==> {response.Code} - {response.Message}");
            }

            response.Value.Country = request.Country;

            return response.Value;
        }
    }
}