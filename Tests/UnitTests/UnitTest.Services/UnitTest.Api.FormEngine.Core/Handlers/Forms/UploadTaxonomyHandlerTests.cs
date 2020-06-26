using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Handlers.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Unit tests for UploadTaxonomyHandler
    /// </summary>
    public class UploadTaxonomyHandlerTests
    {
        private readonly FormsEngineContext formsEngineContext;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadTaxonomyHandlerTests"/> class.
        /// </summary>
        public UploadTaxonomyHandlerTests()
        {
            configuration = UnitTestHelper.GiveServiceProvider().GetRequiredService<IConfiguration>();
            formsEngineContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FormsEngineContext>();
            SetupDB();
        }

        private void SetupDB()
        {
            SetupQsTable();
            SetupTaxonomyTable();
            formsEngineContext.SaveChanges();
        }

        private void SetupQsTable()
        {
            var qs = new QS
            {
                QSId = new Guid("6C6A27DB-D80D-4DE9-7CE6-08D7C0FEB044"),
                QSNo = 76,
                QSVersion = 1,
                StatusId = 1,
                QSName = "Trade Effluent",
                Label = "Trade Effluent",
                Helptext = "Guidance on answering these questions component:",
            };
            formsEngineContext.QS.Add(qs);
        }

        private void SetupTaxonomyTable()
        {
            var taxonomyDictionary = new Dictionary<string, string>
            {
                { "key1", "val1" },
                { "key2", "val2" }
            };
            var tax = new Taxonomy
            {
                TaxonomyID = new Guid("173F6C98-04BD-4D61-3CE8-08D7C4D320C7"),
                Tenant = "England",
                LanguageCode = "English",
                QsNo = 76,
                QsVersion = "1",
                TaxonomyDictionary = JsonConvert.SerializeObject(taxonomyDictionary, Formatting.Indented)
            };
            formsEngineContext.Taxonomy.Add(tax);
        }

        /// <summary>
        /// Unit test for upload taxonomy
        /// </summary>
        /// <returns>Returns the response for taxonomy upload</returns>
        [Fact]
        public async Task UploadTaxonomyCsvTest()
        {
            // Arrange
            var handler = new UploadTaxonomyHandler(formsEngineContext);
            var fileName = "FormEngine:FileNames:TaxonomyWithEnglishAndWelsh";
            var request = new UploadTaxonomy
            {
                FileContent = GetFile(fileName)
            };

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response.Response);
        }

        /// <summary>
        /// Unit test for upload taxonomy without file
        /// </summary>
        /// <returns>Throws an ArgumentNullException for request</returns>
        [Fact]
        public async Task UploadTaxonomyWithNoFileTest()
        {
            // Arrange
            var handler = new UploadTaxonomyHandler(formsEngineContext);
            UploadTaxonomy request = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(request, CancellationToken.None));
        }

        /// <summary>
        /// Unit test for upload taxonomy when file is not a csv
        /// </summary>
        /// <returns>Throws an exception sayig that file is not a csv</returns>
        [Fact]
        public async Task UploadTaxonomyWhenFileIsNotCsvTest()
        {
            // Arrange
            var handler = new UploadTaxonomyHandler(formsEngineContext);
            var fileName = "FormEngine:FileNames:QuestionSetWithNoErrors";
            var request = new UploadTaxonomy
            {
                FileContent = GetFile(fileName)
            };

            // Assert
            await Assert.ThrowsAsync<TQException>(() => handler.Handle(request, CancellationToken.None));
        }

        /// <summary>
        /// Method to read the test taxonomy file
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        /// <returns>IFormFile containing the file content</returns>
        private IFormFile GetFile(string filePath)
        {
            var fileName = configuration[filePath];
            string path = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "TestData", fileName);

            var fileArray = File.ReadAllBytes(path);
            return new FormFile(new MemoryStream(fileArray), 0, fileArray.Length, "Document", fileName);
        }
    }
}