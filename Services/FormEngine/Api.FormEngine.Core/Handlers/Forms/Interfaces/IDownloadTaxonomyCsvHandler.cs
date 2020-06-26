using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.ViewModels;
using MediatR;
using System.Threading.Tasks;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Handlers.Forms.Interfaces
{
    /// <summary>
    /// Interface to download taxonomy as csv
    /// </summary>
    public interface IDownloadTaxonomyCsvHandler : IRequestHandler<DownloadTaxonomyCsv, TaxonomyDownload>
    {
        /// <summary>
        /// Download taxonomy method
        /// </summary>
        /// <param name="request">Taxonomy request object</param>
        /// <param name="formsEngineContext">forms engine-context object</param>
        /// <returns>comma separated string</returns>
        Task<TaxonomyDownload> DownloadTaxonomyCsvAsync(DownloadTaxonomyCsv request, FormsEngineContext formsEngineContext);
    }
}