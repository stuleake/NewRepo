using CT.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.CT.Storage
{
    [ExcludeFromCodeCoverage]
    public class FakeStorageManagerTest
    {
        private readonly IStorageManager storageManager;

        public FakeStorageManagerTest()
        {
            var serviceProvider = UnitTestHelper.GiveServiceProvider();
            storageManager = serviceProvider.GetRequiredService<IStorageManager>();
        }

        private bool CreateContainer(string containerName)
        {
            return storageManager.CreateContainerAsync(containerName).GetAwaiter().GetResult();
        }

        private bool DeleteContainer(string containerName)
        {
            return storageManager.DeleteContainerAsync(containerName).GetAwaiter().GetResult();
        }

        private bool UploadFile(string containerName, string fileName)
        {
            UnicodeEncoding uniEncoding = new UnicodeEncoding();
            byte[] fileArray = uniEncoding.GetBytes("Random file characters! ");
            string file = "testFileName.doc";
            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(file);
            var uri = storageManager.UploadFileToBlobStorageAsync(containerName, fileName, fileArray, mimeType).GetAwaiter().GetResult();
            return uri != null;
        }

        /// <summary>
        /// Test to ctreate container
        /// </summary>
        [Fact]
        public void CreateContainerTest()
        {
            // Arrange
            var containerName = "testContainer";

            // Act
            var result = storageManager.CreateContainerAsync(containerName).GetAwaiter().GetResult();

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
            var containerName = "testContainer";
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
        /// Test to delete files from Container
        /// </summary>
        [Fact]
        public void DeleteFileFromContainerTest()
        {
            // Arrange
            var containerName = "testContainer";
            var fileName = "testFile";
            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }
            if (!UploadFile(containerName, fileName))
            {
                throw new Exception("Couldn't upload file");
            }

            // Act
            var result = storageManager.DeleteAllFilesFromContainerAsync(containerName).GetAwaiter().GetResult();

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to delete single file from container
        /// </summary>
        [Fact]
        public void DeleteFileTest()
        {
            // Arrange
            var fileName = "testFile";
            var containerName = "testContainer";

            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }
            if (!UploadFile(containerName, fileName))
            {
                throw new Exception("Couldn't upload file");
            }

            // Act
            var result = storageManager.DeleteFileFromContainerAsync(containerName, fileName).GetAwaiter().GetResult();

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test to download file
        /// </summary>
        [Fact]
        public void DownloadFileFromBlobTest()
        {
            // Arrange
            var fileName = "testFile";
            var containerName = "testContainer";

            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }
            if (!UploadFile(containerName, fileName))
            {
                throw new Exception("Couldn't upload file");
            }

            // Act
            var result = storageManager.DownloadFileFromBlobStorageAsync(containerName, fileName, true).GetAwaiter().GetResult();

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.NotEqual(0, result.Length);
        }

        /// <summary>
        /// Tets to get File Uri from Test
        /// </summary>
        [Fact]
        public void GetFileUriTest()
        {
            // Arrange
            var fileName = "testFile";
            var containerName = "testContainer";

            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }
            if (!UploadFile(containerName, fileName))
            {
                throw new Exception("Couldn't upload file");
            }

            // Act
            var uri = storageManager.GetFileUriAsync(containerName, fileName, "read|write").GetAwaiter().GetResult();

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.NotNull(uri);
        }

        /// <summary>
        /// Test to get the SaS Token
        /// </summary>
        [Fact]
        public void GetSasTokenTest()
        {
            // Arrange
            var fileName = "testFile";
            var containerName = "testContainer";
            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }
            if (!UploadFile(containerName, fileName))
            {
                throw new Exception("Couldn't upload file");
            }

            // Act
            var token = storageManager.GetSasTokenAsync(containerName, fileName, "read|write|add").GetAwaiter().GetResult();

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.NotNull(token);
        }

        /// <summary>
        /// Tets to upload file
        /// </summary>
        [Fact]
        public void UploadFileTest()
        {
            // Arrange
            var fileName = "testFile";
            var containerName = "testContainer";

            UnicodeEncoding uniEncoding = new UnicodeEncoding();
            byte[] fileArray = uniEncoding.GetBytes("Random file characters! ");
            string file = "testFileName.doc";
            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(file);

            if (!CreateContainer(containerName))
            {
                throw new Exception("Couldn't create container.");
            }

            // Act
            var uri = storageManager.UploadFileToBlobStorageAsync(containerName, fileName, fileArray, mimeType).GetAwaiter().GetResult();

            var act = $"{storageManager.BaseUrl}{containerName}/{fileName}";

            if (!DeleteContainer(containerName))
            {
                throw new Exception("Couldn't delete container.");
            }

            // Assert
            Assert.Equal(act, uri);
        }
    }
}