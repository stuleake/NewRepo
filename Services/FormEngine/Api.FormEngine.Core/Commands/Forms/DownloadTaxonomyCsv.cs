using Api.FormEngine.Core.ViewModels;
using MediatR;

namespace Api.FormEngine.Core.Commands.Forms
{
    /// <summary>
    /// Class for taxonomy download
    /// </summary>
    public class DownloadTaxonomyCsv : BaseCommand, IRequest<TaxonomyDownload>
    {
        /// <summary>
        /// Gets or sets Question set number
        /// </summary>
        public string QsNo { get; set; }

        /// <summary>
        /// Gets or sets Question set version number
        /// </summary>
        public string QsVersion { get; set; }
    }
}