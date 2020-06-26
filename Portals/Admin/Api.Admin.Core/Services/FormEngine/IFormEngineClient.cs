using Api.Admin.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.Admin.Core.Services.FormEngine
{
    /// <summary>
    /// IForm Engine client to perform CRUD operation on Question Set
    /// </summary>
    public interface IFormEngineClient
    {
        /// <summary>
        /// Method declaration to get Question Set
        /// </summary>
        /// <param name="requestUrl">Request Uri for question Set</param>
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
        Task<ServiceResponse<QuestionSetResponse>> GetQuestionSetResponseAsync(string requestUrl, JObject data, string authToken);

        /// <summary>
        /// Method Declaration to create question set response
        /// </summary>
        /// <param name="requestUrl">Request uri for Question set</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>Returns the Id of Question Set</returns>
        Task<ServiceResponse<Guid>> CreateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken);

        /// <summary>
        /// Method declaration of submit Question set
        /// </summary>
        /// <param name="requestUrl">Request Url for question Set</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>Returns the response object of submit operation</returns>
        Task<ServiceResponse<QuestionSetSubmit>> SubmitQuestionSetResponseAsync(string requestUrl, JObject data, string authToken);

        /// <summary>
        /// Method declaration of validate question set response
        /// </summary>
        /// <param name="requestUrl">Request Url for question set</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <returns>Returns the response of validation operation</returns>
        Task<ServiceResponse<QuestionSetSubmit>> ValidateQuestionSetResponseAsync(string requestUrl, JObject data, string authToken);

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
        /// <param name="portal">portal from which the request originated</param>
        /// <returns>Returns the object containing the list of applications.</returns>
        Task<ServiceResponse<ApplicationListModel>> GetDraftApplicationListAsync(string requestUrl, JObject data, string authToken, string portal = null);

        /// <summary>
        /// Method declaration to get message of file
        /// </summary>
        /// <param name="requestUrl">Request Url for question set</param>
        /// <param name="file">File data as IFormFile</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="product">Product</param>
        /// <returns>Return messsage of form structure file</returns>
        Task<ServiceResponse<string>> UploadFormStructureFileAsync(string requestUrl, IFormFile file, string authToken, string product);

        /// <summary>
        /// Method declaration to download taxonomy csv
        /// </summary>
        /// <param name="requestUrl">Request Url for question set</param>
        /// <param name="data">Form data as json object</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="product">Product</param>
        /// <returns>Return messsage of form structure file</returns>
        Task<ServiceResponse<DownloadCsv>> TaxonomyFileDownloadAsync(string requestUrl, JObject data, string authToken, string product);

        /// <summary>
        /// Method declaration to get message for upload of taxonomy file
        /// </summary>
        /// <param name="requestUrl">Request url for taxonomy upload</param>
        /// <param name="file">File data as IFormFile</param>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="product">Product</param>
        /// <returns>Returns message of taxonomy upload</returns>
        Task<ServiceResponse<TaxonomyUploadResponse>> UploadTaxonomyFileAsync(string requestUrl, IFormFile file, string authToken, string product);
    }
}