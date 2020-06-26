using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Planner.Core.Services.Gatsby
{
    /// <summary>
    /// IGatsbyClient to manage Gatsby build
    /// </summary>
    public interface IGatsbyClient
    {
        /// <summary>
        /// Method to call Gatsby builder
        /// </summary>
        /// <param name="requestUrl">Request Url</param>
        /// <param name="data">Gatsby definition data</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="onlyStatus">true is response need only status</param>
        /// <returns>Returns bool value if the builder started or no </returns>
        Task<ServiceResponse<bool>> GatsbyBuildAsync(string requestUrl, JObject data, string authToken, bool onlyStatus);
    }
}