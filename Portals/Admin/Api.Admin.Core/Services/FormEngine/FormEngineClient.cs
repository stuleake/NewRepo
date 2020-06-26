using Api.Admin.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Admin.Core.Services.FormEngine
{
    /// <summary>
    /// Form Engine client to perform CRUD operation on Question Set
    /// </summary>
    public class FormEngineClient : AdminBaseHttpClient, IFormEngineClient
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
        public async Task<ServiceResponse<Guid>> CreateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken)
        {
            return await ExecuteRequestAsync<Guid>(HttpMethod.Post, requestUrl, authToken, data).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<QuestionSet>> GetQuestionSetAsync(string requestUrl, string authToken)
        {
            return await ExecuteRequestAsync<QuestionSet>(HttpMethod.Get, requestUrl, authToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<QuestionSetResponse>> GetQuestionSetResponseAsync(string requestUrl, JObject data, string authToken)
        {
            return await ExecuteRequestAsync<QuestionSetResponse>(HttpMethod.Post, requestUrl, authToken, data).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<QuestionSetSubmit>> SubmitQuestionSetResponseAsync(string requestUrl, JObject data, string authToken)
        {
            return await ExecuteRequestAsync<QuestionSetSubmit>(HttpMethod.Post, requestUrl, authToken, data).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<ServiceResponse<QuestionSetSubmit>> ValidateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken)
        {
            return await ExecuteRequestAsync<QuestionSetSubmit>(HttpMethod.Post, requestUrl, authToken, data).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<ServiceResponse<bool>> DeleteQuestionSetResponseAsync(string requestUrl, string authToken)
        {
            return await ExecuteRequestAsync<bool>(HttpMethod.Delete, requestUrl, authToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<ApplicationListModel>> GetDraftApplicationListAsync(string requestUrl, JObject data, string authToken, string portal = null)
        {
            return await ExecuteRequestAsync<ApplicationListModel>(HttpMethod.Post, requestUrl, authToken, data, country: portal).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<string>> UploadFormStructureFileAsync(string requestUrl, IFormFile file, string authToken, string product)
        {
            return await ExecuteFileRequestAsync<string>(file, requestUrl, authToken, product).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<DownloadCsv>> TaxonomyFileDownloadAsync(string requestUrl, JObject data, string authToken, string product)
        {
            return await ExecuteRequestAsync<DownloadCsv>(
                HttpMethod.Post,
                requestUrl,
                authToken,
                data,
                country: null,
                portal: null,
                logException: false,
                product: product).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<TaxonomyUploadResponse>> UploadTaxonomyFileAsync(string requestUrl, IFormFile file, string authToken, string product)
        {
            return await ExecuteFileRequestAsync<TaxonomyUploadResponse>(file, requestUrl, authToken, product).ConfigureAwait(false);
        }
    }
}