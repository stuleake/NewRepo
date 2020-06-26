using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Globals.Core.Commands.DocumentUpload
{
    /// <summary>
    /// A class to handle response for document upload api
    /// </summary>
    public class DocumentUploadRequest : IRequest<Dictionary<string, string>>
    {
        /// <summary>
        /// Gets or Sets the container name
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or Sets the Name of sub container for document upload
        /// </summary>
        public string SubContainerName { get; set; }

        /// <summary>
        /// Gets or Sets the list of Document as a file
        /// </summary>
        [Required]
        public IEnumerable<IFormFile> Documents { get; set; }

        /// <summary>
        /// Gets or Sets the list of Metadata as dictionary
        /// </summary>
        [Required]
        public IEnumerable<Dictionary<string, string>> Metadatas { get; set; }
    }
}