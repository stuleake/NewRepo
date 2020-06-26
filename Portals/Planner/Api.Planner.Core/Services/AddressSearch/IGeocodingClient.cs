using Api.Planner.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Planner.Core.Services.AddressSearch
{
    /// <summary>
    /// The Geocoding client interface to perform search GET operations on the geocoding address data
    /// </summary>
    public interface IGeocodingClient
    {
        /// <summary>
        /// Returns an enumeration of full address models <see cref="FullAddressSearchModel"/> that match the supplied postcode
        /// </summary>
        /// <param name="requestUrl">the request url to use</param>
        /// <param name="authToken">the authorisation token to use</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ServiceResponse<IEnumerable<FullAddressSearchModel>>> GetFullAddressByPostCodeAsync(string requestUrl, string authToken);

        /// <summary>
        /// Returns an enumeration of simple address models <see cref="SimpleAddressSearchModel"/> that match the supplied postcode
        /// </summary>
        /// <param name="requestUrl">the request url to use</param>
        /// <param name="authToken">the authorisation token to use</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ServiceResponse<IEnumerable<SimpleAddressSearchModel>>> GetSimpleAddressByPostCodeAsync(string requestUrl, string authToken);
    }
}