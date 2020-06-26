using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Planner.Core.Services.Globals
{
    /// <summary>
    /// Manages the Globals Api.
    /// </summary>
    public class GlobalsClient : PlannerBaseHttpClient, IGlobalsClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalsClient"/> class
        /// </summary>
        /// <param name="client">object of HttpClient</param>
        /// <param name="logger">object of Log to log operation in App Insight</param>
        public GlobalsClient(HttpClient client, ILogger<GlobalsClient> logger) : base(client, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<bool>> SendEmailAsync(string requestUrl, JObject data)
        {
            return await ExecuteRequestAsync<bool>(HttpMethod.Post, requestUrl, null, data).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<bool>> CreateUserAsync(string requestUrl, JObject data)
        {
            return await ExecuteRequestAsync<bool>(HttpMethod.Post, requestUrl, null, data).ConfigureAwait(false);
        }
    }
}