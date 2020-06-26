using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace UnitTest.Helpers.FakeResources.FakeStorage
{
    /// <summary>
    /// Fake cloud blob
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeCloudBlob
    {
        /// <summary>
        /// Gets or sets list of fake containers
        /// </summary>
        public List<FakeCloudBlobContainer> FakeContainers { get; set; }

        private readonly Dictionary<string, int> fakeContainersDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCloudBlob"/> class.
        /// Constructor for initialising list of fake container
        /// </summary>
        public FakeCloudBlob()
        {
            FakeContainers = new List<FakeCloudBlobContainer>();
            fakeContainersDictionary = new Dictionary<string, int>();
        }

        /// <summary>
        /// Creates the container
        /// </summary>
        /// <param name="containerName">Takes container name as parameters</param>
        /// <returns>Returns a bool value</returns>
        public async Task<bool> CreateContainerAsync(string containerName)
        {
            if (!fakeContainersDictionary.ContainsKey(containerName))
            {
                FakeContainers.Add(new FakeCloudBlobContainer(containerName));
                fakeContainersDictionary[containerName] = 1;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        /// <summary>
        /// Checks if container exists
        /// </summary>
        /// <param name="containerName">Takes container name as parameters</param>
        /// <returns>Returns a bool value</returns>
        public async Task<bool> ContainerExistsAsync(string containerName)
        {
            return await Task.FromResult(fakeContainersDictionary.ContainsKey(containerName));
        }

        /// <summary>
        /// Deletes all files from container
        /// </summary>
        /// <param name="containerName">Takes container name as parameters</param>
        /// <returns>Returns a bool value</returns>
        public async Task<bool> DeleteAllFilesFromContainerAsync(string containerName)
        {
            if (fakeContainersDictionary.ContainsKey(containerName))
            {
                var container = FakeContainers.Find(name => name.ContainerName == containerName);
                container.FakeBlobs = new List<FakeBlob>();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        /// <summary>
        /// Deletes a container
        /// </summary>
        /// <param name="containerName">Takes container name as parameters</param>
        /// <returns>Returns bool value</returns>
        public async Task<bool> DeleteContainerAsync(string containerName)
        {
            if (fakeContainersDictionary.ContainsKey(containerName))
            {
                FakeContainers.RemoveAll(name => name.ContainerName == containerName);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        /// <summary>
        /// Delete file from container
        /// </summary>
        /// <param name="containerName">Takes container name as parameters</param>
        /// <param name="fileName">Takes file name as parameters</param>
        /// <returns>Returns a bool value</returns>
        public async Task<bool> DeleteFileFromContainerAsync(string containerName, string fileName)
        {
            if (fakeContainersDictionary.ContainsKey(containerName))
            {
                var container = FakeContainers.Find(name => name.ContainerName == containerName);
                container.FakeBlobs.RemoveAll(e => e.BlobName == fileName);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        /// <summary>
        /// Downloads file from blob storage
        /// </summary>
        /// <param name="containerName">Takes container name as parameters</param>
        /// <param name="fileName">Takes file name as parameters</param>
        /// <param name="delete">Takes a bool delete as parameters</param>
        /// <returns>Returns content of file</returns>
        public async Task<MemoryStream> DownloadFileFromBlobStorageAsync(string containerName, string fileName, bool delete)
        {
            if (fakeContainersDictionary.ContainsKey(containerName))
            {
                var container = FakeContainers.Find(name => name.ContainerName == containerName);
                var blob = container.FakeBlobs.Find(f => f.BlobName == fileName);

                if (delete)
                {
                    container.FakeBlobs.Remove(blob);
                }
                return await Task.FromResult(blob.FileContent);
            }
            return await Task.FromResult(new MemoryStream());
        }

        /// <summary>
        /// Gets a file uri
        /// </summary>
        /// <param name="containerName">Takes container name as parameters</param>
        /// <param name="fileName">Takes file name as parameters</param>
        /// <returns>Returns file uri</returns>
        public async Task<string> GetFileUriAsync(string containerName, string fileName)
        {
            if (fakeContainersDictionary.ContainsKey(containerName))
            {
                var container = FakeContainers.Find(name => name.ContainerName == containerName);
                var blob = container.FakeBlobs.Find(f => f.BlobName == fileName);
                return await Task.FromResult(blob.FileUri);
            }
            return await Task.FromResult(string.Empty);
        }

        /// <summary>
        /// Gets sas token of file
        /// </summary>
        /// <param name="containerName">Takes container name as parameters</param>
        /// <param name="fileName">Takes file name as parameters</param>
        /// <returns>Returns sas token of file</returns>
        public async Task<string> GetSasTokenAsync(string containerName, string fileName)
        {
            if (fakeContainersDictionary.ContainsKey(containerName))
            {
                var container = FakeContainers.Find(name => name.ContainerName == containerName);
                var blob = container.FakeBlobs.Find(f => f.BlobName == fileName);
                return await Task.FromResult(blob.SasToken);
            }
            return await Task.FromResult(string.Empty);
        }

        /// <summary>
        /// Uploads file to blob storage
        /// </summary>
        /// <param name="containerName">Takes container name as parameters</param>
        /// <param name="fileName">Takes file name as parameters</param>
        /// <param name="fileArray">Takes file content as parameters</param>
        /// <param name="baseUrl">Takes vase url as parameters</param>
        /// <returns>Returns file uri on upload success</returns>
        public async Task<string> UploadFileToBlobStorageAsync(string containerName, string fileName, byte[] fileArray, string baseUrl)
        {
            if (fakeContainersDictionary.ContainsKey(containerName) && fileArray != null)
            {
                var container = FakeContainers.Find(name => name.ContainerName == containerName);
                var newBlob = new FakeBlob();

                newBlob.BlobName = fileName;
                newBlob.FileUri = baseUrl + containerName + "/" + fileName;
                newBlob.SasToken = Guid.NewGuid().ToString();
                newBlob.FileContent = new MemoryStream();
                newBlob.FileContent.Write(fileArray, 0, fileArray.Length);

                container.FakeBlobs.Add(newBlob);
                return await Task.FromResult(newBlob.FileUri);
            }
            return await Task.FromResult(string.Empty);
        }
    }
}