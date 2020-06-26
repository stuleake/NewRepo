using Api.Planner.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Planner.Core.Services.FormEngine
{
    /// <summary>
    /// Form Engine client to perform CRUD operation on Question Set
    /// </summary>
    public class FormEngineClient : PlannerBaseHttpClient, IFormEngineClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormEngineClient"/> class.
        /// </summary>
        /// <param name="client">object of HttpClient</param>
        /// <param name="logger">object of Log to log operation in App Insight</param>
        public FormEngineClient(HttpClient client, ILogger<FormEngineClient> logger) : base(client, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<Guid>> CreateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken, string country)
        {
            return await ExecuteRequestAsync<Guid>(HttpMethod.Post, requestUrl, authToken, data, country: country).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<QuestionSet>> GetQuestionSetAsync(string requestUrl, string authToken)
        {
            return await ExecuteRequestAsync<QuestionSet>(HttpMethod.Get, requestUrl, authToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<QuestionSetResponse>> GetQuestionSetResponseAsync(string requestUrl, JObject data, string authToken, string country)
        {
            return await ExecuteRequestAsync<QuestionSetResponse>(HttpMethod.Post, requestUrl, authToken, data, country: country).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<QuestionSetSubmit>> SubmitQuestionSetResponseAsync(string requestUrl, JObject data, string authToken)
        {
            return await ExecuteRequestAsync<QuestionSetSubmit>(HttpMethod.Post, requestUrl, authToken, data).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<QuestionSetSubmit>> ValidateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken, string country)
        {
            return await ExecuteRequestAsync<QuestionSetSubmit>(HttpMethod.Post, requestUrl, authToken, data, country: country).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<bool>> DeleteQuestionSetResponseAsync(string requestUrl, string authToken)
        {
            return await ExecuteRequestAsync<bool>(HttpMethod.Delete, requestUrl, authToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<ApplicationListModel>> GetDraftApplicationListAsync(string requestUrl, JObject data, string authToken, string portal)
        {
            return await ExecuteRequestAsync<ApplicationListModel>(HttpMethod.Post, requestUrl, authToken, data, country: null, portal: portal).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<List<ApplicationTypeModel>>> GetApplicationTypesAsync(string requestUrl, string authToken, string country, string product)
        {
            return await ExecuteRequestAsync<List<ApplicationTypeModel>>(HttpMethod.Get, requestUrl, authToken, null, country: country, portal: null, logException: false, onlyStatus: false, product: product)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<List<QuestionSet>>> GetQuestionSetsByApplicationTypeAsync(string requestUrl, string authToken)
        {
            return await ExecuteRequestAsync<List<QuestionSet>>(HttpMethod.Get, requestUrl, authToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<List<QuestionSetWithTaxonomies>>> GetTaxonomiesByApplicationTypeAsync(string requestUrl, JObject data, string authToken, string country, string product)
        {
            return await ExecuteRequestAsync<List<QuestionSetWithTaxonomies>>(
                HttpMethod.Post,
                requestUrl,
                authToken,
                data,
                country: country,
                portal: null,
                logException: false,
                onlyStatus: false,
                product: product)
                .ConfigureAwait(false);
        }
    }
}