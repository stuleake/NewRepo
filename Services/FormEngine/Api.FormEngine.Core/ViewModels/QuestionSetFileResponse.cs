using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels
{
    /// <summary>
    /// Question set file response
    /// </summary>
    public class QuestionSetFileResponse
    {
        /// <summary>
        /// Gets or sets the response of uploaded file
        /// </summary>
        public IEnumerable<string> Error { get; set; }

        /// <summary>
        /// Gets or Sets file name
        /// </summary>
        public string FileName { get; set; }
    }
}