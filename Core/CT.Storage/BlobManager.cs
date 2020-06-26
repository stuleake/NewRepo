using CT.Storage.Enum;
using CT.Storage.Exceptions;
using CT.Storage.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CT.Storage
{
    /// <summary>
    /// Blob Manager
    /// </summary>
    internal sealed class BlobManager : IStorageManager
    {
        /// <inheritdoc/>
        public CloudBlobClient CloudBlobClient { get; set; }

        /// <inheritdoc />
        public int SasExpiryTimeout { get; set; }

        /// <inheritdoc />
        public string BaseUrl
        {
            get
            {
                return CloudBlobClient?.BaseUri?.AbsoluteUri;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobManager"/> class.
        /// </summary>
        /// <param name="storageconnection">Connection string for the blob storage</param>
        /// <param name="connectionType">connection type</param>
        /// <param name="sasexpiry">SAS token expiry</param>
        public BlobManager(string storageconnection, ConnectionTypes connectionType, int sasexpiry)
        {
            switch (connectionType)
            {
                case ConnectionTypes.ConnectionString:
                    var cloudStorageAccount = CloudStorageAccount.Parse(storageconnection);
                    CloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    this.SasExpiryTimeout = sasexpiry;
                    break;

                case ConnectionTypes.MSI:
                    var tokenCredential = new TokenCredential(MsiToken.GetMsiToken());
                    var storageCredentials = new StorageCredentials(tokenCredential);
                    cloudStorageAccount = new CloudStorageAccount(storageCredentials, storageconnection, true);
                    CloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    break;
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetSasTokenAsync(string containername, string fileName, string permissions)
        {
            var cloudBlobContainer = CloudBlobClient.GetContainerReference(containername);
            var exists = await cloudBlobContainer.ExistsAsync().ConfigureAwait(false);

            if (!exists)
            {
                throw new NotFoundException($"Container Does not exits : {containername}");
            }

            string sasToken = string.Empty;
            var sharedAccessbolobPolicy = new SharedAccessBlobPolicy
            {
                Permissions = BlobPersmissionHelper.GetPersmissions(permissions),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(SasExpiryTimeout),
            };

            if (!string.IsNullOrEmpty(fileName))
            {
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                sasToken = cloudBlockBlob.GetSharedAccessSignature(sharedAccessbolobPolicy);
            }
            else
            {
                sasToken = cloudBlobContainer.GetSharedAccessSignature(sharedAccessbolobPolicy);
            }
            return sasToken;
        }

        /// <inheritdoc/>
        public async Task<string> GetFileUriAsync(string containerName, string fileName, string permissions)
        {
            var cloudBlobContainer = CloudBlobClient.GetContainerReference(containerName);

            var exists = await cloudBlobContainer.ExistsAsync().ConfigureAwait(false);

            if (!exists)
            {
                throw new NotFoundException($"Container Does not exits : {containerName}");
            }

            var cloudBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

            if (cloudBlob != null)
            {
                string sasToken = await GetSasTokenAsync(containerName, fileName, permissions).ConfigureAwait(false);
                return cloudBlob.Uri.AbsoluteUri + sasToken;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <inheritdoc/>
        public async Task<string> UploadFileToBlobStorageAsync(string containerName, string filename, byte[] file, string contentType, Dictionary<string, string> metaDataProperties = null)
        {
            var cloudBlobContainer = CloudBlobClient.GetContainerReference(containerName);

            await cloudBlobContainer.CreateIfNotExistsAsync().ConfigureAwait(false);

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);

            cloudBlockBlob.Properties.ContentType = contentType;

            if (metaDataProperties != null && metaDataProperties.Count > 0)
            {
                foreach (var prop in metaDataProperties)
                {
                    cloudBlockBlob.Metadata.Add(prop.Key, prop.Value);
                }
            }
            await cloudBlockBlob.UploadFromByteArrayAsync(file, 0, file.Length).ConfigureAwait(false);

            return cloudBlockBlob.Uri.ToString();
        }

        /// <inheritdoc/>
        public async Task<MemoryStream> DownloadFileFromBlobStorageAsync(string containerName, string fileName, bool delete = false)
        {
            var cloudBlobContainer = CloudBlobClient.GetContainerReference(containerName);
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            var memoryStream = new MemoryStream();
            await cloudBlockBlob.DownloadToStreamAsync(memoryStream).ConfigureAwait(false);

            if (delete)
            {
                await cloudBlockBlob.DeleteAsync().ConfigureAwait(false);
            }

            return memoryStream;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteFileFromContainerAsync(string containerName, string fileName)
        {
            var cloudBlobContainer = CloudBlobClient.GetContainerReference(containerName);
            var blob = cloudBlobContainer.GetBlockBlobReference(fileName);
            var success = await blob.DeleteIfExistsAsync().ConfigureAwait(false);
            return success;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAllFilesFromContainerAsync(string containerName)
        {
            var cloudBlobContainer = CloudBlobClient.GetContainerReference(containerName);
            BlobContinuationToken token = null;

            do
            {
                var result = await cloudBlobContainer.ListBlobsSegmentedAsync(token).ConfigureAwait(false);
                token = result.ContinuationToken;

                var blobs = result.Results;

                Parallel.ForEach(blobs, y =>
                {
                    ((CloudBlockBlob)y).DeleteIfExistsAsync().GetAwaiter();
                });
            }
            while (token != null);

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> CreateContainerAsync(string containerName)
        {
            var cloudBlobContainer = CloudBlobClient.GetContainerReference(containerName);
            return await cloudBlobContainer.CreateIfNotExistsAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteContainerAsync(string containerName)
        {
            var cloudBlobContainer = CloudBlobClient.GetContainerReference(containerName);
            return await cloudBlobContainer.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> ContainerExistsAsync(string containerName)
        {
            var cloudBlobContainer = CloudBlobClient.GetContainerReference(containerName);
            return await cloudBlobContainer.ExistsAsync().ConfigureAwait(false);
        }
    }
}