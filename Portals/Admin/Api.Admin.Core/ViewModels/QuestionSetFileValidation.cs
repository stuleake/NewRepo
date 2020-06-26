using Microsoft.AspNetCore.Http;
using System;

namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// Question set file validation
    /// </summary>
    public class QuestionSetFileValidation
    {
        /// <summary>
        /// Gets or Sets file content
        /// </summary>
        public IFormFile FileContent { get; set; }

        /// <summary>
        /// Gets or Sets the Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets the User Id of the User
        /// </summary>
        public Guid UserId { get; set; }
    }
}