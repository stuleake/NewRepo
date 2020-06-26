using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Admin.Core.Services.FeeCalculator
{
    /// <summary>
    /// Fee calculator client to call fee calculator Apis
    /// </summary>
    public interface IFeeCalculatorClient
    {
        /// <summary>
        /// A method declaration to import the Fee Calculator Rules and save it to the PP2 DB.
        /// </summary>
        /// <param name="requestUrl">Request Url</param>
        /// <param name="data">request data as a json</param>
        /// <param name="country">country name</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ServiceResponse<bool>> ImportFeeCalculatorRulesAsync(string requestUrl, JObject data, string country, string authToken);
    }
}