using Api.FormEngine.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Api.FormEngine.Core.Commands.Forms
{
    /// <summary>
    /// Class to pload taxonomy csv
    /// </summary>
    public class UploadTaxonomy : BaseCommand, IRequest<TaxonomyUploadResponse>
    {
        /// <summary>
        /// Gets or sets csv file content
        /// </summary>
        public IFormFile FileContent { get; set; }
    }
}