using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// class to manage dynamic UI html page
    /// </summary>
    public class DynamicHtmlPage : IDynamicHtmlPage
    {
        /// <inheritdoc/>
        public async Task<string> GetDymanicHtmlPageAsync(string countryUrl)
        {
            string data = string.Empty;
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(countryUrl);
            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = response.CharacterSet == null
                    ? new StreamReader(receiveStream)
                    : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            return await Task.FromResult(data).ConfigureAwait(false);
        }
    }
}