using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.Services.FormEngine;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// Handler class to fetch all taxonomies
    /// </summary>
    public class TaxonomiesByApplicationTypeHandler : IRequestHandler<TaxonomiesByApplicationType, List<QuestionSetWithTaxonomies>>
    {
        private readonly IFormEngineClient formEngineClient;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonomiesByApplicationTypeHandler"/> class.
        /// </summary>
        /// <param name="formEngineClient">form engine client</param>
        /// <param name="configuration">Configuration data</param>
        public TaxonomiesByApplicationTypeHandler(IFormEngineClient formEngineClient, IConfiguration configuration)
        {
            this.formEngineClient = formEngineClient;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<List<QuestionSetWithTaxonomies>> Handle(TaxonomiesByApplicationType request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            ServiceResponse<List<QuestionSetWithTaxonomies>> response;
            try
            {
                response = await formEngineClient.GetTaxonomiesByApplicationTypeAsync(
                         configuration["ApiUri:FormEngine:GetTaxonomiesByApplicationNumber"],
                         JObject.Parse(JsonConvert.SerializeObject(request)),
                         request.AuthToken,
                         request.Country,
                         request.Product).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = $"Failed to fetch question set taxonomies for Application Type Id : {request.ApplicationTypeRefNo}";
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