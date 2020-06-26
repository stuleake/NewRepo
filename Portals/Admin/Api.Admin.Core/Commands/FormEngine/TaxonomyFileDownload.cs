using Api.Admin.Core.ViewModels;
using MediatR;

namespace Api.Admin.Core.Commands.FormEngine
{
    /// <summary>
    /// Class for taxonomy download
    /// </summary>
    public class TaxonomyFileDownload : BaseCommand, IRequest<DownloadCsv>
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