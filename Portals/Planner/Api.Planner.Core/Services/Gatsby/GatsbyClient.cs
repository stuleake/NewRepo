using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Planner.Core.Services.Gatsby
{
    /// <summary>
    /// GatsbyClient to call the Gatsby builder
    /// </summary>
    public class GatsbyClient : PlannerBaseHttpClient, IGatsbyClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GatsbyClient"/> class.
        /// </summary>
        /// <param name="client">Http client</param>
        /// <param name="logger">object of Log to log the operation in App Insight</param>
        public GatsbyClient(HttpClient client, ILogger<GatsbyClient> logger) : base(client, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<bool>> GatsbyBuildAsync(string requestUrl, JObject data, string authToken, bool onlyStatus)
        {
            return await ExecuteRequestAsync<bool>(HttpMethod.Post, requestUrl, authToken, data, country: null, portal: null, logException: false, onlyStatus: onlyStatus).ConfigureAwait(false);
        }
    }
}