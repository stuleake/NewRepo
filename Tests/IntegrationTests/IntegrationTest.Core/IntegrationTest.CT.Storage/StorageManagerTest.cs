using CT.KeyVault;
using CT.Storage;
using CT.Storage.Enum;
using IntegrationTest.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace IntegrationTest.CT.Storage
{
    /// <summary>
    /// Storage Manager Test Client
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StorageManagerTest
    {
        private readonly IConfiguration configuration;
        private readonly IStorageManager storageManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageManagerTest"/> class.
        /// </summary>
        public StorageManagerTest()
        {
            var serviceProvider = IntegrationHelper.GiveServiceProvider();
            configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var vaultManager = serviceProvider.GetRequiredService<IVaultManager>();
            storageManager = StorageProvider.CreateManager(vaultManager.GetSecret(configuration["Storage:StorageAccountUrl"]), ConnectionTypes.ConnectionString, 30);
        }

        private bool CreateContainer(string containerName)
        {
            return storageManager.CloudBlobClient.GetContainerReference(containerName).ExistsAsync().Result
                   || storageManager.CreateContainerAsync(containerName).GetAwaiter().GetResult();
        }

        private bool DeleteContainer(string containerName)
        {
            return storageManager.DeleteContainerAsync(containerName).GetAwaiter().GetResult();
        }

        private bool UploadFile(string containerName)
        {
            var fileName = configuration["Storage:FileName"];
            string path = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), fileName);

            byte[] fileArray = File.ReadAllBytes(path);
            string file = Path.GetFileName(path);
            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(file);
            var uri = storageManager.UploadFileToBlobStorageAsync(containerName, fileName, fileArray, mimeType).GetAwaiter().GetResult();
            return uri != null;
        }

        /// <summary>
        /// Tets to upload file
        /// </summary>
        [Fact]
        public void UploadFileTest()
        {
            // Arrange
            var containerName = $"{configuration["Storage:ContainerName"]}{1}";
            var fileName = configuration["Storage:FileName"];
            string path = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), fileName);

            byte[] fileArray = File.ReadAllBytes(path);
            string file = Path.GetFileName(path);
            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(file);

            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }

            // Act
            var uri = storageManager.UploadFileToBlobStorageAsync(containerName, fileName, fileArray, mimeType).Result;

            var act = $"{storageManager.BaseUrl}{containerName}/{fileName}";

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.Equal(act, uri);
        }

        /// <summary>
        /// Test to get the SaS Token
        /// </summary>
        [Fact]
        public void GetSasTokenTest()
        {
            // Arrange
            var containerName = $"{configuration["Storage:ContainerName"]}{2}";
            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }

            // Act
            var token = storageManager.GetSasTokenAsync(containerName, @"TestFile.docx", "read|write|add").Result;

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.NotNull(token);
        }

        /// <summary>
        /// Test to check invalid file name
        /// </summary>
        [Fact]
        public void GetSasTokenWithInvalidFileNameTest()
        {
            // Arrange
            var containerName = $"{configuration["Storage:ContainerName"]}{3}";
            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }

            // Act
            var token = storageManager.GetSasTokenAsync(containerName, @"TestFile1.docx", "read|write|add").Result;

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.NotNull(token);
        }

        /// <summary>
        /// Test to check invalid SaS Token
        /// </summary>
        [Fact]
        public void GetSasTokenInvalidTest()
        {
            // Act
            var sasToken = string.Empty;
            Exception error = null;
            try
            {
                sasToken = storageManager.GetSasTokenAsync("test", @"TestFile.docx", "read|write|add").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                error = ex;
            }

            // Assert
            Assert.Equal(sasToken, string.Empty);
            Assert.NotNull(error);
            Assert.Equal("Container Does not exits : test", error.Message);
        }

        /// <summary>
        /// Tets to get File Uri from Test
        /// </summary>
        [Fact]
        public void GetFileUriTest()
        {
            // Arrange
            var fileName = configuration["Storage:FileName"];
            var containerName = $"{configuration["Storage:ContainerName"]}{4}";

            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }

            // Act
            var uri = storageManager.GetFileUriAsync(containerName, fileName, "read|write").Result;

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.NotNull(uri);
        }

        /// <summary>
        /// Test to download file
        /// </summary>
        [Fact]
        public void DownloadFileFromBlobTest()
        {
            // Arrange
            var fileName = configuration["Storage:FileName"];
            var containerName = $"{configuration["Storage:ContainerName"]}{5}";

            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }
            if (!UploadFile(containerName))
            {
                throw new Exception("Couldn't upload file");
            }

            // Act
            var result = storageManager.DownloadFileFromBlobStorageAsync(containerName, fileName, true).Result;

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.NotEqual(0, result.Length);
        }

        /// <summary>
        /// Test to delete single file from container
        /// </summary>
        [Fact]
        public void DeleteFileTest()
        {
            // Arrange
            var fileName = configuration["Storage:FileName"];
            var containerName = $"{configuration["Storage:ContainerName"]}{6}";

            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }
            if (!UploadFile(containerName))
            {
                throw new Exception("Couldn't upload file");
            }

            // Act
            var result = storageManager.DeleteFileFromContainerAsync(containerName, fileName).Result;

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to delete files from Container
        /// </summary>
        [Fact]
        public void DeleteFileFromContainerTest()
        {
            // Arrange
            var containerName = $"{configuration["Storage:ContainerName"]}{7}";
            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }
            if (!UploadFile(containerName))
            {
                throw new Exception("Couldn't upload file");
            }

            // Act
            var result = storageManager.DeleteAllFilesFromContainerAsync(containerName).Result;

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to delete container
        /// </summary>
        [Fact]
        public void DeleteContainerTest()
        {
            // Arrange
            var containerName = $"{configuration["Storage:ContainerName"]}{8}";
            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }

            // Act
            var result = storageManager.DeleteContainerAsync(containerName).GetAwaiter().GetResult();

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to ctreate container
        /// </summary>
        [Fact]
        public void CreateContainerTest()
        {
            // Arrange
            var containerName = $"{configuration["Storage:ContainerName"]}{9}";

            // Act
            var result = storageManager.CreateContainerAsync(containerName).GetAwaiter().GetResult();

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.True(result);
        }
    }
}