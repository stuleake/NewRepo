using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Core.Models;

namespace TQ.Core.Clients
{
    /// <summary>
    /// Base http client to manage the base requests
    /// </summary>
    public abstract class BaseHttpClient
    {
        /// <summary>
        /// Gets or sets the logger for the app Insights
        /// </summary>
        protected ILogger<BaseHttpClient> Logger { get; set; }

        /// <summary>
        /// Gets or sets the http client for performing actions
        /// </summary>
        protected HttpClient Client { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHttpClient"/> class
        /// </summary>
        /// <param name="httpClient">Http client required for the setup</param>
        /// <param name="logger">Instance of logger required for setup</param>
        protected BaseHttpClient(HttpClient httpClient, ILogger<BaseHttpClient> logger)
        {
            Client = httpClient;
            Logger = logger;
        }

        /// <summary>
        /// Generate the request
        /// </summary>
        /// <param name="method">The verb for the request</param>
        /// <param name="requestUrl">The Url for the request</param>
        /// <param name="authToken">The authToken to be used for the request</param>
        /// <param name="customHeaders">The custom Headers for the request</param>
        /// <param name="entity">The payload for the request</param>
        /// <returns>A HttpRequestMessage representing the request</returns>
        protected static HttpRequestMessage GenerateRequest(HttpMethod method, string requestUrl, string authToken, Dictionary<string, object> customHeaders = null, JObject entity = null)
        {
            var request = new HttpRequestMessage(method, requestUrl);

            // Add bearer token to the headers
            if (!string.IsNullOrEmpty((authToken ?? string.Empty).Trim()))
            {
                request.Headers.Add(Constants.ApplicationConstants.Authorization, authToken);
            }

            // Add custom headers to the request
            if (customHeaders != null && customHeaders.Any())
            {
                foreach (var header in customHeaders)
                {
                    request.Headers.Add(header.Key, Convert.ToString(header.Value));
                }
            }

            // Add the request content
            if (entity != null)
            {
                request.Content = new StringContent(entity.ToString(), Encoding.UTF8, "application/json");
            }

            return request;
        }

        /// <summary>
        /// A method to process response
        /// </summary>
        /// <typeparam name="TEntity">type of object</typeparam>
        /// <param name="responseMessage">object of http response</param>
        /// <param name="onlystatus">onlystatus needed or content</param>
        /// <returns>Returns Service Response of type T</returns>
        public async Task<ServiceResponse<TEntity>> ProcessResponseAsync<TEntity>(HttpResponseMessage responseMessage, bool onlystatus = false)
        {
            if (responseMessage == null)
            {
                throw new ArgumentNullException(nameof(responseMessage));
            }

            var response = new ServiceResponse<TEntity>
            {
                Code = (int)responseMessage.StatusCode,
                Value = default
            };

            try
            {
                if (!onlystatus)
                {
                    response = await ProcessResponseCodesAsync<TEntity>(responseMessage, response).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                var message = $"Failed to map the response to type '{typeof(TEntity).Name}'";
                throw new MappingResponseException($"{message}", ex);
            }

            return response;
        }

        /// <summary>
        /// Process response codes
        /// </summary>
        /// <typeparam name="TEntity">type of object</typeparam>
        /// <param name="httpresponse">response from the client</param>
        /// <param name="serviceResponse">service response generated</param>
        /// <returns>The response object from the client</returns>
        protected abstract Task<ServiceResponse<TEntity>> ProcessResponseCodesAsync<TEntity>(
            HttpResponseMessage httpresponse, ServiceResponse<TEntity> serviceResponse);
    }
}