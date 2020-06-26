using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Admin.Core.Services.FeeCalculator
{
    /// <summary>
    /// A FeeCalculator client to call services from Fee Calculator
    /// </summary>
    public class FeeCalculatorClient : AdminBaseHttpClient, IFeeCalculatorClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeeCalculatorClient"/> class.
        /// </summary>
        /// <param name="client">Http client</param>
        /// <param name="logger">object of Log to log the operation in App Insight</param>
        public FeeCalculatorClient(HttpClient client, ILogger<FeeCalculatorClient> logger) : base(client, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<bool>> ImportFeeCalculatorRulesAsync(string requestUrl, JObject data, string country, string authToken)
        {
            return await ExecuteRequestAsync<bool>(HttpMethod.Post, requestUrl, authToken, data, country: country, portal: null, logException: true).ConfigureAwait(false);
        }
    }
}