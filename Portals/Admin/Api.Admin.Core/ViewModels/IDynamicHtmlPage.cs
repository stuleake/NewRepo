using System.Threading.Tasks;

namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// The IDynamicHtmlPage interface
    /// </summary>
    public interface IDynamicHtmlPage
    {
        /// <summary>
        /// get the Html page from the URL
        /// </summary>
        /// <param name="countryUrl">country specific html Page URL</param>
        /// <returns>HTML page content as string</returns>
        Task<string> GetDymanicHtmlPageAsync(string countryUrl);
    }
}
