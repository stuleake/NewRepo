using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Clients;
using TQ.Core.Constants;
using TQ.Core.Exceptions;
using TQ.Core.Models;

namespace Api.Planner.Core.Services
{
    /// <summary>
    /// PLanner BaseHttpClient
    /// </summary>
    public abstract class PlannerBaseHttpClient : BaseHttpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlannerBaseHttpClient"/> class.
        /// </summary>
        /// <param name="httpClient">client to send requests</param>
        /// <param name="logger">Logger class to log custom events</param>
        protected PlannerBaseHttpClient(HttpClient httpClient, ILogger<PlannerBaseHttpClient> logger) : base(httpClient, logger)
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

            switch (httpresponse.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                    {
                        var strValue = await httpresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        serviceResponse.Value = JsonConvert.DeserializeObject<TEntity>(strValue);
                    }
                    break;

                case HttpStatusCode.BadRequest:
                    {
                        var strValue = await httpresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        dynamic result = JsonConvert.DeserializeObject<dynamic>(strValue);
                        serviceResponse.Message = result.message;
                    }
                    break;

                default:
                    {
                        serviceResponse.Message = httpresponse.ReasonPhrase;
                    }
                    break;
            }

            return serviceResponse;
        }

        /// <summary>
        /// A method to create Authenticated request
        /// </summary>
        /// <typeparam name="TEntity">Entity reposenting the response type</typeparam>
        /// <param name="httpMethod">Http method type</param>
        /// <param name="requestUrl">request url</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="entity">Entity as json object</param>
        /// <param name="country">Country for which the request has to be processed</param>
        /// <param name="portal">Portal from which the request generated</param>
        /// <param name="logException">Boolean to log custom exception or not</param>
        /// <param name="onlyStatus">Boolean to check response content is needed or no</param>
        /// <param name="product">Product for which the request has to be processed</param>
        /// <returns>A <see cref="Task{TEntity}"/> representing the result of the asynchronous operation.</returns>
        protected async virtual Task<ServiceResponse<TEntity>> ExecuteRequestAsync<TEntity>(HttpMethod httpMethod,
            string requestUrl,
            string authToken,
            JObject entity = null,
            string country = null,
            string portal = null,
            bool logException = false,
            bool onlyStatus = false,
            string product = null)
        {
            if (httpMethod == null)
            {
                throw new ArgumentNullException($"{nameof(httpMethod)}");
            }

            if (string.IsNullOrWhiteSpace(requestUrl))
            {
                throw new ArgumentNullException($"{nameof(requestUrl)}");
            }

            // Setting up custom headers
            var customHeaders = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(country))
            {
                customHeaders.Add(ApplicationConstants.Country, country);
            }

            if (!string.IsNullOrWhiteSpace(portal))
            {
                customHeaders.Add(ApplicationConstants.Portal, portal);
            }

            if (!string.IsNullOrWhiteSpace(product))
            {
                customHeaders.Add(ApplicationConstants.Product, product);
            }

            try
            {
                var request = GenerateRequest(httpMethod, requestUrl, authToken, customHeaders, entity);
                var response = await Client.SendAsync(request).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var responseContent = response.Content.ReadAsStringAsync();
                    var badResult = JsonConvert.DeserializeObject<BadResultModel>(responseContent.Result);
                    throw new TQException(badResult.Message);
                }
                return await ProcessResponseAsync<TEntity>(response, onlyStatus).ConfigureAwait(false);
            }
            catch (TQException)
            {
                throw;
            }
            catch (MappingResponseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var message = $"Failed to process '{System.Reflection.MethodBase.GetCurrentMethod().Name}'";
                if (logException)
                {
                    Logger.LogCritical(ex, message);
                    throw new ServiceException($"{message}", ex);
                }

                throw new ServiceException($"{message}", ex);
            }
        }
    }
}