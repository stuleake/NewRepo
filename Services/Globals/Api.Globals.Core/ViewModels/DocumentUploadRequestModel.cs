using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Globals.Core.ViewModels
{
    /// <summary>
    /// Request model to upload document which takes metadata as a json string
    /// </summary>
    public class DocumentUploadRequestModel
    {
        /// <summary>
        /// Gets or Sets the container name
        /// </summary>
        [Required]
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or Sets the Name of sub container for document upload
        /// </summary>
        public string SubContainerName { get; set; }

        /// <summary>
        /// Gets or Sets the Document as a file
        /// </summary>
        public IEnumerable<IFormFile> Documents { get; set; }

        /// <summary>
        /// Gets or Sets the metadata as a serialized json string
        /// </summary>
        public string Metadatas { get; set; }
    }
}