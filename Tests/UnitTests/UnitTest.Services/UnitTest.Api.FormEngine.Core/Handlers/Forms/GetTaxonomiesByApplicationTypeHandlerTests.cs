using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Handlers.Forms;
using Api.FormEngine.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Unit tests for Taxonomies By Application Type Handler Test
    /// </summary>
    public class GetTaxonomiesByApplicationTypeHandlerTests
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTaxonomiesByApplicationTypeHandlerTests"/> class.
        /// </summary>
        public GetTaxonomiesByApplicationTypeHandlerTests()
        {
            formsEngineContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FormsEngineContext>();
            SetupDB();
        }

        private void SetupDB()
        {
            SetupQsCollectionTypes();
            SetupQsCollectionMappings();
            SetupQsTable();
            SetupTaxonomyTable();
            formsEngineContext.SaveChanges();
        }

        private void SetupQsCollectionMappings()
        {
            var qsmapping = new QSCollectionMapping
            {
                QSCollectionMappingId = new Guid("bbaf852f-f015-4711-9cd6-5ed4fd2fbb2a"),
                QSCollectionTypeId = new Guid("81e2754a-add7-4231-87a0-c7d67ff42c7f"),
                QSId = new Guid("45eb99b4-baf3-4a70-a591-64ee2b3a91fa"),
                QSNo = 45,
                Sequence = 1
            };
            var qsmapping1 = new QSCollectionMapping
            {
                QSCollectionMappingId = new Guid("dcd1b5a9-a15b-45b2-b07c-7da8b1afd4ce"),
                QSCollectionTypeId = new Guid("81e2754a-add7-4231-87a0-c7d67ff42c7f"),
                QSId = new Guid("a2522f6b-3b18-48b3-afd4-c6df67344edb"),
                QSNo = 46,
                Sequence = 1
            };
            formsEngineContext.QSCollectionMapping.Add(qsmapping);
            formsEngineContext.QSCollectionMapping.Add(qsmapping1);
        }

        private void SetupQsCollectionTypes()
        {
            var qscollection = new QSCollectionType
            {
                QSCollectionTypeId = new Guid("81e2754a-add7-4231-87a0-c7d67ff42c7f"),
                Label = "test label",
                Description = "This is my description",
                CountryCode = "England",
                CreatedDate = DateTime.ParseExact("2020-03-08 17:11:54", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                LastModifiedDate = DateTime.ParseExact("2020-03-08 17:11:54", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                LastModifiedBy = new Guid("89e67eae-3757-43e8-992d-120d856abb91"),
                ApplicationTypeRefNo = 27,
                QSCollectionVersion = 1.0,
                Tenant = "England",
                Product = "PP2"
            };
            formsEngineContext.QSCollectionType.Add(qscollection);
        }

        private void SetupQsTable()
        {
            var qs = new QS
            {
                QSId = new Guid("45eb99b4-baf3-4a70-a591-64ee2b3a91fa"),
                QSNo = 45,
                QSVersion = 1.0M,
                QSName = "Trade Effluent",
                Label = "Trade Effluent",
                Product = "PP2",
                Tenant = "England",
                Helptext = "Guidance on answering these questions component:",
                StatusId = 2
            };
            var qs1 = new QS
            {
                QSId = new Guid("a2522f6b-3b18-48b3-afd4-c6df67344edb"),
                QSNo = 46,
                QSVersion = 1.0M,
                QSName = "Trade Effluent new",
                Label = "Trade Effluent new",
                Product = "PP2",
                Tenant = "England",
                Helptext = "Guidance on answering these questions component: new",
                StatusId = 2
            };
            formsEngineContext.QS.Add(qs);
            formsEngineContext.QS.Add(qs1);
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
                Product = "PP2",
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
                Product = "PP2",
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
                Product = "PP2",
                TaxonomyDictionary = JsonConvert.SerializeObject(d3, Formatting.Indented)
            };
            formsEngineContext.Taxonomy.Add(tax1);
            formsEngineContext.Taxonomy.Add(tax2);
            formsEngineContext.Taxonomy.Add(tax3);
        }

        /// <summary>
        /// Unit test for taxonomy fetch by given application ref no and language
        /// </summary>
        /// <returns>list of taxonomies</returns>
        [Fact]
        public async Task GetTaxonomiesByApplicationNumberPresentInDB()
        {
            // Arrange
            var handler = new GetTaxonomiesByApplicationTypeHandler(formsEngineContext);

            var request = new GetTaxonomiesByApplicationType
            {
                ApplicationTypeRefNo = 27,
                Language = "English",
                Product = "PP2",
                Country = "England"
            };

            // Act
            var response = await handler.Handle(request, System.Threading.CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Any());
        }

        /// <summary>
        /// Unit test for taxonomy fetch by given application ref no and language
        /// </summary>
        /// <returns>list of taxonomies</returns>
        [Fact]
        public async Task GetTaxonomiesByApplicationNumberNotPresentInDB()
        {
            // Arrange
            var handler = new GetTaxonomiesByApplicationTypeHandler(formsEngineContext);
            Exception error = null;
            List<QuestionSetWithTaxonomies> response = null;

            var request = new GetTaxonomiesByApplicationType
            {
                ApplicationTypeRefNo = 57,
                Language = "English",
                Country = "England",
                Product = "PP2"
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
        /// Unit test for taxonomy fetch with null request
        /// </summary>
        /// <returns>list of taxonomies</returns>
        [Fact]
        public async Task GetTaxonomiesByApplicationNumberNullRequest()
        {
            // Arrange
            var handler = new GetTaxonomiesByApplicationTypeHandler(formsEngineContext);
            Exception error = null;
            List<QuestionSetWithTaxonomies> response = null;

            GetTaxonomiesByApplicationType request = null;

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