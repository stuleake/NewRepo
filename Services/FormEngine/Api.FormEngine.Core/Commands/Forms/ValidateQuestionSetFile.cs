using MediatR;
using Microsoft.AspNetCore.Http;

namespace Api.FormEngine.Core.Commands.Forms
{
    /// <summary>
    /// Class to Get Excel File Validation Error Message
    /// </summary>
    public class ValidateQuestionSetFile : BaseCommand, IRequest<string>
    {
        /// <summary>
        /// Gets or Sets File Content of zip or excel file
        /// </summary>
        public IFormFile FileContent { get; set; }
    }
}