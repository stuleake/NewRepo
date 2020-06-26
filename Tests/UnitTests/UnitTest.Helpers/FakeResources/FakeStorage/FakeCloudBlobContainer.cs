using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UnitTest.Helpers.FakeResources.FakeStorage
{
    /// <summary>
    /// Fake cloud blob container
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeCloudBlobContainer
    {
        /// <summary>
        /// Gets or sets list of fake blobs
        /// </summary>
        public List<FakeBlob> FakeBlobs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCloudBlobContainer"/> class.
        /// Constructor to initialise list of fake blobs
        /// </summary>
        /// <param name="containerName">Takes container name as parameter</param>
        public FakeCloudBlobContainer(string containerName)
        {
            FakeBlobs = new List<FakeBlob>();
            ContainerName = containerName;
        }

        /// <summary>
        /// Gets or sets fake container name
        /// </summary>
        public string ContainerName { get; set; }
    }
}