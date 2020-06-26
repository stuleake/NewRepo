using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Clients;
using TQ.Core.Constants;
using TQ.Core.Exceptions;
using TQ.Core.Models;

namespace Api.Admin.Core.Services
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
                        serviceResponse.Message = await httpresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    break;
            }

            return serviceResponse;
        }

        /// <summary>
        /// A method to create Authenticated request
        /// </summary>
        /// <typeparam name="TEntity">Entity representing the response type</typeparam>
        /// <param name="httpMethod">Http method type</param>
        /// <param name="requestUrl">request url</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="entity">Entity as json object</param>
        /// <param name="country">Country for which the request has to be processed</param>
        /// <param name="portal">Portal from which the request generated</param>
        /// <param name="logException">Boolean to log custom exception or not</param>
        /// <param name="product">Product from which the request generated</param>
        /// <returns>A <see cref="Task{TEntity}"/> representing the result of the asynchronous operation.</returns>
        protected async virtual Task<ServiceResponse<TEntity>> ExecuteRequestAsync<TEntity>(HttpMethod httpMethod,
            string requestUrl,
            string authToken,
            JObject entity = null,
            string country = null,
            string portal = null,
            bool logException = false,
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
                return await ProcessResponseAsync<TEntity>(response).ConfigureAwait(false);
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

        /// <summary>
        /// A method to create Authenticated request for file
        /// </summary>
        /// <typeparam name="TEntity">Entity representing the response type</typeparam>
        /// <param name="file">file details</param>
        /// <param name="requestUrl">request url</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="product">Product</param>
        /// <returns>A <see cref="Task{TEntity}"/> representing the result of the asynchronous operation.</returns>
        protected async virtual Task<ServiceResponse<TEntity>> ExecuteFileRequestAsync<TEntity>(
            IFormFile file,
            string requestUrl,
            string authToken,
            string product)
        {
            try
            {
                if (file == null)
                {
                    throw new ArgumentNullException(nameof(file));
                }

                byte[] data;
                using (var br = new BinaryReader(file.OpenReadStream()))
                {
                    data = br.ReadBytes((int)file.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                var multiContent = new MultipartFormDataContent
                {
                    { bytes, "fileContent", file.FileName }
                };

                var request = GenerateRequest(HttpMethod.Post, requestUrl, authToken);
                request.Content = multiContent;
                request.Headers.Add(ApplicationConstants.Product, product);
                var response = await Client.SendAsync(request).ConfigureAwait(false);
                return await ProcessResponseAsync<TEntity>(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
            }
        }
    }
}