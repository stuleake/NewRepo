using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Planner.Core.Services.Globals
{
    /// <summary>
    /// IGlobalsClient.
    /// </summary>
    public interface IGlobalsClient
    {
        /// <summary>
        /// Create Sendemail Request and response from Globals
        /// </summary>
        /// <param name="requestUrl">Request uri for globals</param>
        /// <param name="data">Data to send the email</param>
        /// <returns>bool</returns>
        Task<ServiceResponse<bool>> SendEmailAsync(string requestUrl, JObject data);

        /// <summary>
        /// create user in azure
        /// </summary>
        /// <param name="requestUrl">Request url for creating a user</param>
        /// <param name="data">Payload of the user to be created</param>
        /// <returns>Returns a value representing the response</returns>
        Task<ServiceResponse<bool>> CreateUserAsync(string requestUrl, JObject data);
    }
}