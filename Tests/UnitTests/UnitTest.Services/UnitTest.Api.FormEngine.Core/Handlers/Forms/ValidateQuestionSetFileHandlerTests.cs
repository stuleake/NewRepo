using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Handlers.Forms;
using Api.FormEngine.Core.Services.Globals;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.Services.Processors;
using Api.FormEngine.Core.Services.Validators;
using Api.FormEngine.Core.ViewModels.SheetModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Exceptions;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;
using UnitTest.Helpers;
using UnitTest.Helpers.FakeClients;
using Xunit;
using Section = Api.FormEngine.Core.ViewModels.SheetModels.Section;

namespace UnitTest.Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Unit tests for question set file validation handler
    /// </summary>
    public class ValidateQuestionSetFileHandlerTests
    {
        private readonly FormsEngineContext formsEngineContext;
        private readonly IConfiguration configuration;

        private readonly IGlobalClient fakeGlobalClient;

        private readonly IValidator<ExcelSheetsData> mockExcelSheetValidator = Mock.Create<IValidator<ExcelSheetsData>>();
        private readonly IProcessor<IEnumerable<Aggregations>> mockFieldAggregationProcessor = Mock.Create<IProcessor<IEnumerable<Aggregations>>>();
        private readonly IProcessor<FieldConstraintParserModel> mockFieldConstraintProcessor = Mock.Create<IProcessor<FieldConstraintParserModel>>();
        private readonly IProcessor<QuestionSetParserModel> mockQuestionSetProcessor = Mock.Create<IProcessor<QuestionSetParserModel>>();
        private readonly IProcessor<SectionParserModel> mockSectionProcessor = Mock.Create<IProcessor<SectionParserModel>>();
        private readonly IProcessor<SectionMappingParserModel> mockSectionMappingProcessor = Mock.Create<IProcessor<SectionMappingParserModel>>();
        private readonly IProcessor<FieldParserModel> mockFieldProcessor = Mock.Create<IProcessor<FieldParserModel>>();
        private readonly IProcessor<TaxonomyParserModel> mockTaxonomyProcessor = Mock.Create<IProcessor<TaxonomyParserModel>>();
        private readonly IProcessor<IEnumerable<Section>> mockSectionFieldMappingProcessor = Mock.Create<IProcessor<IEnumerable<Section>>>();

        private readonly string[] display = { "Optional", "Hide", "Required" };

        private readonly string[] fieldType =
        {
            "NUMBER",
            "DROPDOWN",
            "DATE",
            "RADIOGROUP",
            "BUTTON",
            "ActionInput",
            "ActionAddress",
            "ActionTable",
            "Aggregation",
            "TEXT",
            "NUMBERSELECTOR",
            "Notification"
        };

        private readonly string[] constraints =
        {
            "Solo",
            "Depends",
            "Decides"
        };

        private readonly string[] answerTypes =
        {
            "Range",
            "Length",
            "Regex",
            "RegexBE",
            "Multiple",
            "Value",
            "API"
        };

        private readonly string[] functions =
        {
            "SUM",
            "DIFF",
            "COUNT",
            "AVG",
            "MIN",
            "MAX"
        };

        private readonly string[] sectionType = { "Main-Fields", "Main-Table", "Sub-Fields", "Sub-Table" };
        private readonly string[] rules = { "Any", "All" };

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateQuestionSetFileHandlerTests"/> class.
        /// </summary>
        public ValidateQuestionSetFileHandlerTests()
        {
            formsEngineContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FormsEngineContext>();
            fakeGlobalClient = new FakeGlobalsClient().GlobalsClient;
            configuration = UnitTestHelper.GiveServiceProvider().GetRequiredService<IConfiguration>();
            SetupDB();
        }

        private void SetupDB()
        {
            int count = 0;
            foreach (var item in display)
            {
                count += 1;
                formsEngineContext.Displays.Add(new TQ.Data.FormEngine.Schemas.Forms.Display
                {
                    DisplayId = count,
                    Displays = item
                });
            }
            count = 0;
            foreach (var item in fieldType)
            {
                count += 1;
                formsEngineContext.FieldTypes.Add(new TQ.Data.FormEngine.Schemas.Forms.FieldType
                {
                    FieldTypeId = count,
                    FieldTypes = item
                });
            }
            count = 0;
            foreach (var item in constraints)
            {
                count += 1;
                formsEngineContext.Constraints.Add(new TQ.Data.FormEngine.Schemas.Forms.Constraint
                {
                    ConstraintId = count,
                    Constraints = item
                });
            }
            count = 0;
            foreach (var item in answerTypes)
            {
                count += 1;
                formsEngineContext.AnswerTypes.Add(new TQ.Data.FormEngine.Schemas.Forms.AnswerType
                {
                    AnswerTypeId = count,
                    AnswerTypes = item
                });
            }
            count = 0;
            foreach (var item in functions)
            {
                count += 1;
                formsEngineContext.Functions.Add(new TQ.Data.FormEngine.Schemas.Forms.Function
                {
                    FunctionsId = count,
                    Functions = item
                });
            }
            count = 0;
            foreach (var item in sectionType)
            {
                count += 1;
                formsEngineContext.SectionTypes.Add(new TQ.Data.FormEngine.Schemas.Forms.SectionType
                {
                    SectionTypeId = count,
                    SectionTypes = item
                });
            }
            count = 0;
            foreach (var item in rules)
            {
                count += 1;
                formsEngineContext.Rules.Add(new TQ.Data.FormEngine.Schemas.Forms.Rule
                {
                    RuleId = count,
                    Rules = item
                });
            }
            AddQuestionSet(2, 76);
            AddQuestionSet(1, 77);
            formsEngineContext.SaveChanges();
        }

        private void AddQuestionSet(int quesSetStatus, int quesSetNumber)
        {
            var quesSetId = Guid.NewGuid();
            formsEngineContext.QS.Add(new QS
            {
                QSId = quesSetId,
                QSNo = quesSetNumber,
                Tenant = "England",
                Language = "English",
                QSVersion = 1.0M,
                Label = "Pre-application Advice",
                Helptext = "1",
                Description = string.Empty,
                StatusId = quesSetStatus,
            });

            var sectionList = new List<TQ.Data.FormEngine.Schemas.Forms.Section>();
            sectionList.Add(new TQ.Data.FormEngine.Schemas.Forms.Section
            {
                SectionId = Guid.NewGuid(),
                SectionNo = 1,
                Label = "Section label",
                Helptext = "Section Helptext",
                Description = "Section Description",
                SectionTypeId = 1
            });
            formsEngineContext.Section.AddRange(sectionList);
            foreach (var section in sectionList)
            {
                formsEngineContext.QSSectionMapping.Add(new QSSectionMapping
                {
                    QSSectionMappingId = Guid.NewGuid(),
                    SectionId = section.SectionId,
                    QSId = quesSetId,
                });

                var fieldList = new List<TQ.Data.FormEngine.Schemas.Forms.Field>();
                fieldList.Add(new TQ.Data.FormEngine.Schemas.Forms.Field
                {
                    FieldId = Guid.NewGuid(),
                    FieldNo = 4,
                    Label = "Surname",
                    Helptext = string.Empty,
                    Description = string.Empty,
                    FieldTypeId = 3
                });

                fieldList.Add(new TQ.Data.FormEngine.Schemas.Forms.Field
                {
                    FieldId = Guid.NewGuid(),
                    FieldNo = 2,
                    Label = "Title",
                    Helptext = string.Empty,
                    Description = string.Empty,
                    FieldTypeId = 4
                });
                fieldList.Add(new TQ.Data.FormEngine.Schemas.Forms.Field
                {
                    FieldId = Guid.NewGuid(),
                    FieldNo = 11,
                    Label = "Address",
                    Helptext = string.Empty,
                    Description = string.Empty,
                    FieldTypeId = 4
                });
                formsEngineContext.Field.AddRange(fieldList);

                foreach (var field in fieldList)
                {
                    formsEngineContext.SectionFieldMapping.Add(new SectionFieldMapping
                    {
                        SectionFieldMappingId = Guid.NewGuid(),
                        FieldId = field.FieldId,
                        SectionId = section.SectionId,
                        FieldNo = field.FieldNo
                    });
                }
            }
        }

        /// <summary>
        /// Unit test for GetQuestionSetFileResponseAsync with no errors
        /// </summary>
        /// <returns>errors if any</returns>
        [Fact]
        public async Task GetQuestionSetFileResponseAsync_WithNoErrorsTest()
        {
            // Arrange
            Exception error = null;
            var fileName = "FormEngine:FileNames:QuestionSetWithNoErrors";
            IFormFile formFileDocument = GetFile(fileName);

            ValidateQuestionSetFile request = new ValidateQuestionSetFile
            {
                FileContent = formFileDocument
            };

            var handler = CreateSut();
            Mock.Arrange(() => mockExcelSheetValidator.Validate(null)).IgnoreArguments().Returns(new ValidationResult<ExcelSheetsData>(new ExcelSheetsData()));

            // Act
            try
            {
                _ = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (TQException er)
            {
                error = er;
            }

            // Assert
            Assert.Null(error);
        }

        /// <summary>
        /// Unit test for GetQuestionSetFileResponseAsync with no errors versioning With Active Question Set
        /// </summary>
        /// <returns>errors if any</returns>
        [Fact]
        public async Task GetQuestionSetFileResponseAsync_WithNoErrorsVersioningTest()
        {
            // Arrange
            Exception error = null;
            var fileName = "FormEngine:FileNames:QuestionSetWithNoErrorsVersioning";
            IFormFile formFileDocument = GetFile(fileName);

            ValidateQuestionSetFile request = new ValidateQuestionSetFile
            {
                FileContent = formFileDocument
            };

            var handler = CreateSut();
            Mock.Arrange(() => mockExcelSheetValidator.Validate(null)).IgnoreArguments().Returns(new ValidationResult<ExcelSheetsData>(new ExcelSheetsData()));

            // Act
            try
            {
                _ = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (TQException er)
            {
                error = er;
            }

            // Assert
            Assert.Null(error);
        }

        /// <summary>
        /// Unit test for GetQuestionSetFileResponseAsync with no errors versioning With Draft Question Set
        /// </summary>
        /// <returns>errors if any</returns>
        [Fact]
        public async Task GetQuestionSetFileResponseAsync_WithNoErrorsVersioningDraftTest()
        {
            // Arrange
            Exception error = null;
            var fileName = "FormEngine:FileNames:QuestionSetWithNoErrorsVersioningDraft";
            IFormFile formFileDocument = GetFile(fileName);

            ValidateQuestionSetFile request = new ValidateQuestionSetFile
            {
                FileContent = formFileDocument
            };

            var handler = CreateSut();
            Mock.Arrange(() => mockExcelSheetValidator.Validate(null)).IgnoreArguments().Returns(new ValidationResult<ExcelSheetsData>(new ExcelSheetsData()));

            // Act
            try
            {
                _ = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (TQException er)
            {
                error = er;
            }

            // Assert
            Assert.Null(error);
        }

        /// <summary>
        /// Unit test for GetQuestionSetFileResponseAsync with errors
        /// </summary>
        /// <returns>errors if any</returns>
        [Fact]
        public async Task GetQuestionSetFileResponseAsync_WithErrorsTest()
        {
            // Arrange
            Exception error = null;
            var fileName = "FormEngine:FileNames:QuestionSetWithErrors";

            IFormFile formFileDocument = GetFile(fileName);

            ValidateQuestionSetFile request = new ValidateQuestionSetFile
            {
                FileContent = formFileDocument
            };

            var handler = CreateSut();

            // Act
            try
            {
                _ = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (TQException er)
            {
                error = er;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for GetQuestionSetFileResponseAsync with zip file
        /// </summary>
        /// <returns>errors if any</returns>
        [Fact]
        public async Task GetQuestionSetFileResponseAsync_ZipFileTest()
        {
            // Arrange
            Exception error = null;
            var fileName = "FormEngine:FileNames:QuestionSetZip";

            IFormFile formFileDocument = GetFile(fileName);

            ValidateQuestionSetFile request = new ValidateQuestionSetFile
            {
                FileContent = formFileDocument
            };

            var handler = CreateSut();

            // Act
            try
            {
                _ = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (TQException er)
            {
                error = er;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for GetQuestionSetFileResponseAsync with invalid file format
        /// </summary>
        /// <returns>errors if any</returns>
        [Fact]
        public async Task GetQuestionSetFileResponseAsync_InvalidFileFormatTest()
        {
            // Arrange
            Exception error = null;
            var fileName = "FormEngine:FileNames:PngFile";

            IFormFile formFileDocument = GetFile(fileName);

            ValidateQuestionSetFile request = new ValidateQuestionSetFile
            {
                FileContent = formFileDocument
            };

            var handler = CreateSut();

            // Act
            try
            {
                _ = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (TQException er)
            {
                error = er;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for GetQuestionSetFileResponseAsync with invalid header format
        /// </summary>
        /// <returns>errors if any</returns>
        [Fact]
        public async Task GetQuestionSetFileResponseAsync_InvalidHeaderFormatTest()
        {
            // Arrange
            Exception error = null;
            var fileName = "FormEngine:FileNames:InvalidHeaders";

            IFormFile formFileDocument = GetFile(fileName);

            var request = new ValidateQuestionSetFile
            {
                FileContent = formFileDocument
            };

            var handler = CreateSut();

            // Act
            try
            {
                _ = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (TQException er)
            {
                error = er;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for GetQuestionSetFileResponseAsync_InvalidNumberOfExcelSheetsTest with invalid number of sheets
        /// </summary>
        /// <returns>errros if any</returns>
        [Fact]
        public async Task GetQuestionSetFileResponseAsync_InvalidNumberOfExcelSheetsTest()
        {
            // Arrange
            Exception error = null;
            var fileName = "FormEngine:FileNames:InvalidNumberOfExcelSheets";

            IFormFile formFileDocument = GetFile(fileName);

            var request = new ValidateQuestionSetFile
            {
                FileContent = formFileDocument
            };

            var handler = CreateSut();

            // Act
            try
            {
                _ = await handler.Handle(request, System.Threading.CancellationToken.None);
            }
            catch (TQException er)
            {
                error = er;
            }

            // Assert
            Assert.NotNull(error);
        }

        private IFormFile GetFile(string filePath)
        {
            var fileName = configuration[filePath];
            string path = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "TestData", fileName);

            byte[] fileArray = File.ReadAllBytes(path);
            string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            IFormFile formFileDocument = new FormFile(new MemoryStream(fileArray), 0, fileArray.Length, "Document", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType
            };
            return formFileDocument;
        }

        private ValidateQuestionSetFileHandler CreateSut()
        {
            return new ValidateQuestionSetFileHandler(
                formsEngineContext,
                fakeGlobalClient,
                configuration,
                mockExcelSheetValidator,
                mockFieldAggregationProcessor,
                mockSectionFieldMappingProcessor,
                mockFieldConstraintProcessor,
                mockQuestionSetProcessor,
                mockSectionProcessor,
                mockSectionMappingProcessor,
                mockFieldProcessor,
                mockTaxonomyProcessor);
        }
    }
}