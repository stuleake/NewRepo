using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Handlers.Forms;
using Api.FormEngine.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Unit tests for question set Taxonomy Csv Download Handler
    /// </summary>
    public class DownloadTaxonomyCsvHandlerTests
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadTaxonomyCsvHandlerTests"/> class.
        /// </summary>
        public DownloadTaxonomyCsvHandlerTests()
        {
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
                QSId = new Guid("3FD8A1C8-C932-436E-F468-08D7C363C59A"),
                QSNo = 45,
                QSVersion = 1.0M,
                QSName = "Trade Effluent",
                Label = "Trade Effluent",
                Helptext = "Guidance on answering these questions component:",
            };
            var qs1 = new QS
            {
                QSId = new Guid("16F6451C-6306-472A-706B-08D7C36A40A7"),
                QSNo = 46,
                QSVersion = 1.0M,
                QSName = "Trade Effluent new",
                Label = "Trade Effluent new",
                Helptext = "Guidance on answering these questions component: new",
            };
            var qs2 = new QS
            {
                QSId = new Guid("ED64EA08-C2C8-4BAE-AA76-E8984876743F"),
                QSNo = 48,
                QSVersion = 1.0M,
                QSName = "Trade Effluent new1",
                Label = "Trade Effluent new1",
                Helptext = "Guidance on answering these questions component: new1",
            };
            formsEngineContext.QS.Add(qs);
            formsEngineContext.QS.Add(qs1);
            formsEngineContext.QS.Add(qs2);
        }

        private void SetupTaxonomyTable()
        {
            var d1 = new Dictionary<string, string>
            {
                { "key1", "val1" },
                { "key2", "val2" },
                { "key3", "val3" }
            };
            var tax1 = new Taxonomy
            {
                TaxonomyID = new Guid("D7FF9950-5BF1-4E22-ACEE-08D7C363C640"),
                Tenant = "England",
                LanguageCode = "English",
                QsNo = 45,
                QsVersion = "1.0",
                TaxonomyDictionary = JsonConvert.SerializeObject(d1, Formatting.Indented)
            };
            var d2 = new Dictionary<string, string>
            {
                { "key1", "val1" },
                { "key2", "val2" }
            };
            var tax2 = new Taxonomy
            {
                TaxonomyID = new Guid("CD944107-4108-4B62-D134-08D7C383801F"),
                Tenant = "England",
                LanguageCode = "Wales",
                QsNo = 45,
                QsVersion = "1.0",
                TaxonomyDictionary = JsonConvert.SerializeObject(d2, Formatting.Indented)
            };
            var d3 = new Dictionary<string, string>
            {
                { "key1", "val1" },
                { "key2", "val2" }
            };
            var tax3 = new Taxonomy
            {
                TaxonomyID = new Guid("76032C0B-5F01-4DF4-106A-08D7C387E076"),
                Tenant = "England",
                LanguageCode = "English",
                QsNo = 46,
                QsVersion = "1.0",
                TaxonomyDictionary = JsonConvert.SerializeObject(d3, Formatting.Indented)
            };
            formsEngineContext.Taxonomy.Add(tax1);
            formsEngineContext.Taxonomy.Add(tax2);
            formsEngineContext.Taxonomy.Add(tax3);
        }

        /// <summary>
        /// Unit test for DownloadTaxonomyCsvAsync with Qs number and qs version
        /// </summary>
        /// <returns>csv string of taxonomy</returns>
        [Fact]
        public async Task DownloadTaxonomyCsvAsync_WithQsNoAndQsVersion()
        {
            // Arrange
            var handler = new DownloadTaxonomyCsvHandler(formsEngineContext);

            var request = new DownloadTaxonomyCsv
            {
                QsNo = "45",
                QsVersion = "1.0"
            };

            // Act
            var response = await handler.Handle(request, System.Threading.CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.NotEqual(string.Empty, response.CsvString);
        }

        /// <summary>
        /// Unit test for DownloadTaxonomyCsvAsync without Qs number and qs version
        /// </summary>
        /// <returns>csv string of taxonomy of all qs numbers</returns>
        [Fact]
        public async Task DownloadTaxonomyCsvAsync_WithoutQsNoAndQsVersion()
        {
            // Arrange
            var handler = new DownloadTaxonomyCsvHandler(formsEngineContext);

            var request = new DownloadTaxonomyCsv();

            // Act
            var response = await handler.Handle(request, System.Threading.CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.NotEqual(string.Empty, response.CsvString);
        }

        /// <summary>
        /// Unit test for DownloadTaxonomyCsvAsync with invalid Qs number and qs version
        /// </summary>
        /// <returns>errors if any</returns>
        [Fact]
        public async Task DownloadTaxonomyCsvAsync_WithInvalidQsNoAndQsVersion()
        {
            // Arrange
            var handler = new DownloadTaxonomyCsvHandler(formsEngineContext);
            Exception error = null;
            TaxonomyDownload response = null;

            var request = new DownloadTaxonomyCsv
            {
                QsNo = "54",
                QsVersion = "2.5"
            };

            // Act
            try
            {
                response = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (TQException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
            Assert.Null(response);
        }

        /// <summary>
        /// Unit test for DownloadTaxonomyCsvAsync without forms engine context
        /// </summary>
        /// <returns>errors if any</returns>
        [Fact]
        public async Task DownloadTaxonomyCsvAsync_WithoutFormsEngineContext()
        {
            // Arrange
            var handler = new DownloadTaxonomyCsvHandler(null);
            Exception error = null;
            TaxonomyDownload response = null;

            var request = new DownloadTaxonomyCsv();

            // Act
            try
            {
                response = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
            Assert.Null(response);
        }
    }
}