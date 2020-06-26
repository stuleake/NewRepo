using Api.Planner.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Planner.Core.Services.FormEngine
{
    /// <summary>
    /// IForm Engine client to perform CRUD operation on Question Set
    /// </summary>
    public interface IFormEngineClient
    {
        /// <summary>
        /// Method declaration to get Question Set
        /// </summary>
        /// <param name="requestUrl">Request Url for question Set</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>Returns Question Set</returns>
        Task<ServiceResponse<QuestionSet>> GetQuestionSetAsync(string requestUrl, string authToken);

        /// <summary>
        /// Method declaration to get question set response
        /// </summary>
        /// <param name="requestUrl">Request Url for Question Set</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>Returns the Question Set Response</returns>
        Task<ServiceResponse<QuestionSetResponse>> GetQuestionSetResponseAsync(string requestUrl, JObject data, string authToken, string country);

        /// <summary>
        /// Method declaration of submit Question set
        /// </summary>
        /// <param name="requestUrl">Request Url for question Set</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>Returns the response object of submit operation</returns>
        Task<ServiceResponse<QuestionSetSubmit>> SubmitQuestionSetResponseAsync(string requestUrl, JObject data, string authToken);

        /// <summary>
        /// Method Declaration to create question set response
        /// </summary>
        /// <param name="requestUrl">Request url for Question set</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="country">Country of the Caller</param>
        /// <returns>Returns the Id of Question Set</returns>
        Task<ServiceResponse<Guid>> CreateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken, string country);

        /// <summary>
        /// Method declaration of validate question set response
        /// </summary>
        /// <param name="requestUrl">Request Url for question set</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>Returns the response of validation operation</returns>
        Task<ServiceResponse<QuestionSetSubmit>> ValidateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken, string country);

        /// <summary>
        /// Method declaration of Delete question set response
        /// </summary>
        /// <param name="requestUrl">Request Url for Question Set</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>Returns boolean for successful Deletion</returns>
        Task<ServiceResponse<bool>> DeleteQuestionSetResponseAsync(string requestUrl, string authToken);

        /// <summary>
        /// A method declaration to get list of draft applications for a User.
        /// </summary>
        /// <param name="requestUrl">Request Url for question set</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="portal">Portal from which the request is generated</param>
        /// <returns>Returns the object containing the list of applications.</returns>
        Task<ServiceResponse<ApplicationListModel>> GetDraftApplicationListAsync(string requestUrl, JObject data, string authToken, string portal);

        /// <summary>
        /// Get ApplicationT ypes
        /// </summary>
        /// <param name="requestUrl">Request Url</param>
        /// <param name="authToken"> The Authentication Token</param>
        /// <param name="country">Country of the Caller</param>
        /// <param name="product">Product of the Caller</param>
        /// <returns>List of Application type</returns>
        Task<ServiceResponse<List<ApplicationTypeModel>>> GetApplicationTypesAsync(string requestUrl, string authToken, string country, string product);

        /// <summary>
        /// Get all the Question Set of the Application Type
        /// </summary>
        /// <param name="requestUrl">The Request Url</param>
        /// <param name="authToken">The Authentication TOken</param>
        /// <returns><A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<ServiceResponse<List<QuestionSet>>> GetQuestionSetsByApplicationTypeAsync(string requestUrl, string authToken);

        /// <summary>
        /// Get all the Question Set taxonomies of the Application Number
        /// </summary>
        /// <param name="requestUrl">The Request Url</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">The Authentication TOken</param>
        /// <param name="country">Country of the Caller</param>
        /// <param name="product">Product of the Caller</param>
        /// <returns>list of object containing the taxonomies</returns>
        Task<ServiceResponse<List<QuestionSetWithTaxonomies>>> GetTaxonomiesByApplicationTypeAsync(string requestUrl, JObject data, string authToken, string country, string product);
    }
}