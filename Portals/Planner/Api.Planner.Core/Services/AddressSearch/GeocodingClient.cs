using Api.Planner.Core.ViewModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Planner.Core.Services.AddressSearch
{
    /// <summary>
    /// Geocoding client to perform search GET operations on the geocoding address data
    /// </summary>
    public class GeocodingClient : PlannerBaseHttpClient, IGeocodingClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingClient"/> class.
        /// </summary>
        /// <param name="client">the http client to use</param>
        /// <param name="logger">the logger to use</param>
        public GeocodingClient(HttpClient client, ILogger<GeocodingClient> logger) : base(client, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<IEnumerable<FullAddressSearchModel>>> GetFullAddressByPostCodeAsync(string requestUrl, string authToken)
        {
            return await ExecuteRequestAsync<IEnumerable<FullAddressSearchModel>>(HttpMethod.Get, requestUrl, authToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<IEnumerable<SimpleAddressSearchModel>>> GetSimpleAddressByPostCodeAsync(string requestUrl, string authToken)
        {
            return await ExecuteRequestAsync<IEnumerable<SimpleAddressSearchModel>>(HttpMethod.Get, requestUrl, authToken).ConfigureAwait(false);
        }
    }
}