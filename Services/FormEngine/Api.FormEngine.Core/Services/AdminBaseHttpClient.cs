using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Clients;
using TQ.Core.Models;

namespace Api.FormEngine.Core.Services
{
    /// <summary>
    /// BaseHttpClient
    /// </summary>
    public abstract class AdminBaseHttpClient : BaseHttpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminBaseHttpClient"/> class.
        /// </summary>
        /// <param name="httpClient">client to send requests</param>
        /// <param name="logger">Logger class to log custom events</param>
        protected AdminBaseHttpClient(HttpClient httpClient, ILogger<AdminBaseHttpClient> logger) : base(httpClient, logger)
        {
        }

        /// <inheritdoc/>
        protected override async Task<ServiceResponse<TEntity>> ProcessResponseCodesAsync<TEntity>(
            HttpResponseMessage httpresponse, ServiceResponse<TEntity> serviceResponse)
        {
            if (httpresponse == null)
            {
                throw new ArgumentNullException(nameof(httpresponse));
            }

            if (serviceResponse == null)
            {
                throw new ArgumentNullException(nameof(serviceResponse));
            }

            if (httpresponse.StatusCode == HttpStatusCode.OK || httpresponse.StatusCode == HttpStatusCode.Created)
            {
                var strValue = await httpresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                serviceResponse.Value = JsonConvert.DeserializeObject<TEntity>(strValue);
            }
            else
            {
                serviceResponse.Message = httpresponse.ReasonPhrase;
            }

            return serviceResponse;
        }

        /// <summary>
        /// A method to create Authenticated request for file
        /// </summary>
        /// <typeparam name="TEntity">Entity representing the response type</typeparam>
        /// <param name="multicontent">multi content file</param>
        /// <param name="requestUrl">request url</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>A <see cref="Task{TEntity}"/> representing the result of the asynchronous operation.</returns>
        protected async virtual Task<ServiceResponse<TEntity>> ExecuteFileRequestAsync<TEntity>(
            MultipartFormDataContent multicontent,
            string requestUrl,
            string authToken)
        {
            if (multicontent == null)
            {
                throw new ArgumentNullException(nameof(multicontent));
            }

            if (string.IsNullOrWhiteSpace(requestUrl))
            {
                throw new ArgumentNullException($"{nameof(requestUrl)}");
            }

            var request = GenerateRequest(HttpMethod.Post, requestUrl, authToken);
            request.Content = multicontent;
            var response = await Client.SendAsync(request).ConfigureAwait(false);

            return await ProcessResponseAsync<TEntity>(response).ConfigureAwait(false);
        }
    }
}