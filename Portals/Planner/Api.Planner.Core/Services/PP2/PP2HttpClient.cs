using Api.Planner.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Planner.Core.Services.PP2
{
    /// <summary>
    /// PP2HttpClient to manage Question Set
    /// </summary>
    public class PP2HttpClient : PlannerBaseHttpClient, IPP2HttpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PP2HttpClient"/> class.
        /// </summary>
        /// <param name="client">Http client</param>
        /// <param name="logger">object of Log to log the operation in App Insight</param>
        public PP2HttpClient(HttpClient client, ILogger<PP2HttpClient> logger) : base(client, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<Guid>> CreateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken, string country)
        {
            return await ExecuteRequestAsync<Guid>(HttpMethod.Post, requestUrl, authToken, data, country: country, portal: null, logException: true).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<ApplicationListModel>> GetSubmittedApplicationsAsync(string requestUrl, string authToken, string country, string portal)
        {
            return await ExecuteRequestAsync<ApplicationListModel>(HttpMethod.Get, requestUrl, authToken, null, country: country, portal: portal, logException: true).ConfigureAwait(false);
        }
    }
}