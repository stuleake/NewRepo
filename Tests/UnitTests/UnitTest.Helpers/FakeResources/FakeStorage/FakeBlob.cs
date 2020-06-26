using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace UnitTest.Helpers.FakeResources.FakeStorage
{
    /// <summary>
    /// Class for creating fake blob
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeBlob
    {
        /// <summary>
        /// Gets or sets blob name
        /// </summary>
        public string BlobName { get; set; }

        /// <summary>
        /// Gets or sets blob uri
        /// </summary>
        public string FileUri { get; set; }

        /// <summary>
        /// Gets or sets blob Sas token
        /// </summary>
        public string SasToken { get; set; }

        /// <summary>
        /// Gets or sets blob file content
        /// </summary>
        public MemoryStream FileContent { get; set; }

        /// <summary>
        /// Gets or sets blob metadata
        /// </summary>
        public IDictionary<string, string> Metadata { get; set; }
    }
}