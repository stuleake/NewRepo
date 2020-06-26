using CT.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using UnitTest.Helpers.FakeResources.FakeStorage;

namespace UnitTest.Helpers.FakeResources
{
    /// <summary>
    /// Fake storgae manager
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class FakeStorageManager : IStorageManager
    {
        /// <inheritdoc/>
        public CloudBlobClient CloudBlobClient { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public int SasExpiryTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        public string BaseUrl { get; set; } = "https://fake-storage-url/";

        private readonly FakeCloudBlob fakeCloudBlob;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeStorageManager"/> class.
        /// Constructor for fake storage manager
        /// </summary>
        public FakeStorageManager()
        {
            fakeCloudBlob = new FakeCloudBlob();
        }

        /// <inheritdoc/>
        public async Task<bool> ContainerExistsAsync(string containerName)
        {
            return await fakeCloudBlob.ContainerExistsAsync(containerName).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> CreateContainerAsync(string containerName)
        {
            return await fakeCloudBlob.CreateContainerAsync(containerName).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAllFilesFromContainerAsync(string containerName)
        {
            return await fakeCloudBlob.DeleteAllFilesFromContainerAsync(containerName).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteContainerAsync(string containerName)
        {
            return await fakeCloudBlob.DeleteContainerAsync(containerName).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteFileFromContainerAsync(string containerName, string fileName)
        {
            return await fakeCloudBlob.DeleteFileFromContainerAsync(containerName, fileName).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<MemoryStream> DownloadFileFromBlobStorageAsync(string containerName, string fileName, bool delete = false)
        {
            return await fakeCloudBlob.DownloadFileFromBlobStorageAsync(containerName, fileName, delete);
        }

        /// <inheritdoc/>
        public async Task<string> GetFileUriAsync(string containerName, string fileName, string permissions)
        {
            return await fakeCloudBlob.GetFileUriAsync(containerName, fileName);
        }

        /// <inheritdoc/>
        public async Task<string> GetSasTokenAsync(string containername, string fileName, string permissions)
        {
            return await fakeCloudBlob.GetSasTokenAsync(containername, fileName);
        }

        /// <inheritdoc/>
        public async Task<string> UploadFileToBlobStorageAsync(string containerName, string filename, byte[] file, string contentType, Dictionary<string, string> metaDataProperties = null)
        {
            return await fakeCloudBlob.UploadFileToBlobStorageAsync(containerName, filename, file, BaseUrl);
        }
    }
}