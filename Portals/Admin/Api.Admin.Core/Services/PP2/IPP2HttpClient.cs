using Api.Admin.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Admin.Core.Services.PP2
{
    /// <summary>
    /// IPP2HttpClient to manage Question Set
    /// </summary>
    public interface IPP2HttpClient
    {
        /// <summary>
        /// Method declaration to create question set response
        /// </summary>
        /// <param name="requestUrl">Request Url</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="country">Country name</param>
        /// <returns>Returns the Question Set ID</returns>
        Task<ServiceResponse<Guid>> CreateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken, string country);

        /// <summary>
        /// A method declaration to get list of submitted applications for a user.
        /// </summary>
        /// <param name="requestUrl">Request Url</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="country">Country name</param>
        /// <param name="portal">portal name</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ServiceResponse<ApplicationListModel>> GetSubmittedApplicationsAsync(string requestUrl, string authToken, string country, string portal);
    }
}