using Api.Admin.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Api.Admin.Core.Commands.FormEngine
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