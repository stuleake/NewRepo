using MediatR;
using Microsoft.AspNetCore.Http;

namespace Api.Admin.Core.Commands.FormEngine
{
    /// <summary>
    /// Command for importing form structure file
    /// </summary>
    public class UploadFormStructureFile : BaseCommand, IRequest<string>
    {
        /// <summary>
        /// Gets or Sets the file
        /// </summary>
        public IFormFile FileContent { get; set; }
    }
}