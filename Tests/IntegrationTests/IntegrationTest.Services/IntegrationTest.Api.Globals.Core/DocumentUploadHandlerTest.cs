using Api.Globals.Core.Commands.DocumentUpload;
using Api.Globals.Core.Handlers.DocumentUpload;
using CT.KeyVault;
using CT.Storage;
using CT.Storage.Enum;
using IntegrationTest.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.Api.Globals.Core
{
    /// <summary>
    /// class for document upload handler test
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DocumentUploadHandlerTest
    {
        private readonly IConfiguration configuration;
        private readonly IStorageManager storageManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentUploadHandlerTest"/> class.
        /// </summary>
        public DocumentUploadHandlerTest()
        {
            var serviceProvider = IntegrationHelper.GiveServiceProvider();
            configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var vaultManager = serviceProvider.GetRequiredService<IVaultManager>();
            storageManager = StorageProvider.CreateManager(vaultManager.GetSecret(configuration["Storage:StorageAccountUrl"]), ConnectionTypes.ConnectionString, 30);
        }

        /// <summary>
        /// Method for document upload unit test
        /// </summary>
        /// <returns>true if success</returns>
        [Fact]
        public async Task Handle_DocumentUpload()
        {
            var fileName = configuration["Storage:FileName"];
            string path = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), fileName);

            byte[] fileArray = File.ReadAllBytes(path);
            string file = Path.GetFileName(path);
            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(file);
            IFormFile formFileDocument = new FormFile(new MemoryStream(fileArray), 0, fileArray.Length, "Document", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType
            };
            var metadata = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "description", "This is test data for description" }, { "comments", "This is test data for comments" } }
            };
            DocumentUploadRequest request = new DocumentUploadRequest
            {
                ContainerName = "testupload",
                SubContainerName = "testformid",
                Documents = new List<IFormFile> { formFileDocument },
                Metadatas = metadata
            };

            // Arrange
            DocumentUploadHandler handler = new DocumentUploadHandler(storageManager);

            // Act
            var response = await handler.Handle(request, System.Threading.CancellationToken.None);
            _ = $"{storageManager.BaseUrl}{request.ContainerName}/{request.SubContainerName}/{request.Documents.ElementAt(0).FileName}";

            // Assert
            Assert.True(response.Count > 0);
        }
    }
}