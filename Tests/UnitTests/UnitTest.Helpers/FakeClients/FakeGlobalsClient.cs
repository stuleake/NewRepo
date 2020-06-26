using Api.FormEngine.Core.Services.Globals;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Models;

namespace UnitTest.Helpers.FakeClients
{
    /// <summary>
    /// Class for fake globalclient
    /// </summary>
    public class FakeGlobalsClient
    {
        /// <summary>
        /// Gets globalclient
        /// </summary>
        public IGlobalClient GlobalsClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeGlobalsClient"/> class.
        /// </summary>
        public FakeGlobalsClient()
        {
            GlobalsClient = Mock.Create<IGlobalClient>();
            Mock.Arrange(() => GlobalsClient.UploadFileAsync(Arg.IsAny<MultipartFormDataContent>(), Arg.IsAny<string>(), Arg.IsAny<string>()))
                .Returns((MultipartFormDataContent multicontent, string requestUrl, string authToken) => UploadFileAsync(multicontent));
        }

        private async Task<ServiceResponse<Dictionary<string, string>>> UploadFileAsync(MultipartFormDataContent multicontent)
        {
            var filesUris = new Dictionary<string, string>();
            if (multicontent.Count<HttpContent>() > 3)
            {
                int i = 1;
                while (i < multicontent.Count<HttpContent>() - 1)
                {
                    filesUris.Add($"{multicontent.ElementAt(++i).Headers.ContentDisposition.FileName.Trim('"')}", $"Fake Url:{i}");
                    i += 1;
                }
            }
            var result = new ServiceResponse<Dictionary<string, string>>
            {
                Value = filesUris
            };
            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}