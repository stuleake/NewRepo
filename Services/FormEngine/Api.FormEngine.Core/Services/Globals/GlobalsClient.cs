using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TQ.Core.Models;

namespace Api.FormEngine.Core.Services.Globals
{
    /// <summary>
    /// Manages the Globals Api.
    /// </summary>
    public class GlobalsClient : AdminBaseHttpClient, IGlobalClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalsClient"/> class
        /// </summary>
        /// <param name="client">object of HttpClient</param>
        /// <param name="logger">object of Log to log operation in App Insight</param>
        public GlobalsClient(HttpClient client, ILogger<GlobalsClient> logger) : base(client, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<Dictionary<string, string>>> UploadFileAsync(MultipartFormDataContent multicontent, string requestUrl, string authToken)
        {
            return await ExecuteFileRequestAsync<Dictionary<string, string>>(multicontent, requestUrl, authToken).ConfigureAwait(false);
        }
    }
}