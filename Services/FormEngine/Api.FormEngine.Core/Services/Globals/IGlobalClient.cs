using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.FormEngine.Core.Services.Globals
{
    /// <summary>
    /// global client interface to upload file in blob
    /// </summary>
    public interface IGlobalClient
    {
        /// <summary>
        /// Trigger global file to upload file in blob storage
        /// </summary>
        /// <param name="multicontent">file details in multi content</param>
        /// <param name="requestUrl">request url of global upload method</param>
        /// <param name="authToken">authorization token</param>
        /// <returns>return list of uploaded file url</returns>
        Task<ServiceResponse<Dictionary<string, string>>> UploadFileAsync(MultipartFormDataContent multicontent, string requestUrl, string authToken);
    }
}