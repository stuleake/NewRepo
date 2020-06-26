using Api.FormEngine.Core.SheetValidators;
using Api.FormEngine.Core.ViewModels.SheetModels;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;
using Xunit;
using AnswerGuide = Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide;
using Field = Api.FormEngine.Core.ViewModels.SheetModels.Field;
using Section = Api.FormEngine.Core.ViewModels.SheetModels.Section;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators
{
    /// <summary>
    /// Field Sheet Validation Tests
    /// </summary>
    public class FieldSheetValidatorTests
    {
        private readonly ExcelSheetsData sheetData;
        private readonly FormStructureData formStructure;
        private readonly FieldSheetValidator validator;
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSheetValidatorTests"/> class.
        /// </summary>
        public FieldSheetValidatorTests()
        {
            validator = CreateSut();
            sheetData = CreateSheetData();
            formStructure = CreateFormStructure();
            formsEngineContext = Mock.Create<FormsEngineContext>();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsyncSuccess should pass minimum information to ExecuteSheetAsync
        /// Data that is passed is also matched in the form structure
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsyncSuccess()
        {
            // Arrange

            // Act
            var result = await validator.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsyncFail_WithErrorMissMatchedInformation should pass minimum information to ExecuteSheetAsync
        /// Data that is passed is also matched in the form structure
        /// Expected Result:
        /// Invalid section rule found. Section rule : Rule
        /// Invalid Section Type : Main-Field in Section Number : 2
        /// Invalid Field Type : TEXT in Field Number : 2
        /// Invalid Field Display : 2 in Field Number : 2
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsyncFail_WithErrorMissMatchedInformation()
        {
            // Arrange
            var questionSet = CreateQuestionSet("2", "2", "TEXT", "2", "Main-Field", "Rule", "2");
            sheetData.QuestionSet = new List<QuestionSet> { questionSet };

            // Act
            var resultLines = await ReturnTestAsListAsync();

            // Assert
            resultLines.Should().BeOfType<List<string>>()
                .And.NotBeEmpty()
                .And.HaveCount(4);
        }

        /// <summary>
        /// Execution of ExecuteSheetAsyncFail_WithErrorMessageInvalidSection should pass minimum information to ExecuteSheetAsync
        /// excluding section information.
        /// Expected Results:
        /// Invalid Section Type :  in Section Number :
        /// Section cannot be null or empty
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsyncFail_WithErrorMessageInvalidSection()
        {
            // Arrange
            var questionSet = CreateQuestionSet("1", "1", "RADIOGROUP", "1", string.Empty, string.Empty, string.Empty);

            sheetData.QuestionSet = new List<QuestionSet> { questionSet };

            // Act
            var resultLines = await ReturnTestAsListAsync();

            // Assert
            resultLines.Should().BeOfType<List<string>>()
                .And.NotBeEmpty()
                .And.HaveCount(2);
        }

        /// <summary>
        /// Execution of ExecuteSheetAsyncFail_WithErrorMessageInvalidFieldDisplay should pass minimum information to ExecuteSheetAsync
        /// excluding Field Display from Field
        /// Expected Result:
        /// "Invalid Field Display :  in Field Number : 1"
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsyncFail_WithErrorMessageInvalidFieldDisplay()
        {
            // Arrange
            sheetData.QuestionSet.First().Sections = new List<Section>
            {
                new Section
                {
                    Fields = new List<Field>
                    {
                        new Field
                        {
                            FieldNo = "1",
                            FieldType = "RADIOGROUP"
                        }
                    },
                    SectionType = "Main-FIelds",
                    SectionRule = "Section Rule",
                    SectionNo = "1"
                }
            };

            // Act
            var resultLines = await ReturnTestAsListAsync();

            // Assert
            resultLines.Should().BeOfType<List<string>>().And.NotBeEmpty();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsyncFail_WithErrorMessageInvalidFieldType should pass minimum information to ExecuteSheetAsync
        /// excluding Field Type from Field
        /// Expected Result:
        /// "Invalid Field Type :  in Field Number : 1"
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsyncFail_WithErrorMessageInvalidFieldType()
        {
            // Arrange
            sheetData.QuestionSet.First().Sections = new List<Section>
            {
                new Section
                {
                    Fields = new List<Field>
                    {
                        new Field
                        {
                            FieldNo = "1",
                            FieldType = string.Empty,
                            FieldDisplay = "1"
                        }
                    },
                    SectionType = "Main-FIelds",
                    SectionRule = "Section Rule",
                    SectionNo = "1"
                }
            };

            // Act
            var resultLines = await ReturnTestAsListAsync();

            // Assert
            resultLines.Should().BeOfType<List<string>>()
                .And.NotBeEmpty()
                .And.HaveCount(1);
        }

       /// <summary>
        /// Execution of ExecuteSheetAsyncFail_WithErrorMessageNullExceptionErrorWithNoSection should pass minimum information to ExecuteSheetAsync
        /// excluding Section Types from FormStructure
        /// Expected result:
        /// NullExceptionError
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]

        public async Task ExecuteSheetAsyncFail_WithErrorMessageNullExceptionErrorWithNoSection()
        {
            // Arrange
            formStructure.SectionTypes = null;

            // Act & Assert
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => validator.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext));
        }

        /// <summary>
        /// Execution of ExecuteSheetAsyncFail_WithErrorMessageNullExceptionErrorWithNoFieldType should pass minimum information to ExecuteSheetAsync
        /// excluding Field Types from FormStructure
        /// Expected result:
        /// NullExceptionError
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsyncFail_WithErrorMessageNullExceptionErrorWithNoFieldType()
        {
            // Arrange
            formStructure.FieldTypes = null;

            // Act & Assert
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => validator.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext));
        }

        /// <summary>
        /// Execution of ExecuteSheetAsyncFail_WithErrorMessageNullExceptionErrorWithNoDisplays should pass minimum information to ExecuteSheetAsync
        /// excluding Displays from FormStructure
        /// Expected result:
        /// NullExceptionError
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]

        public async Task ExecuteSheetAsyncFail_WithErrorMessageNullExceptionErrorWithNoDisplays()
        {
            // Arrange
            formStructure.Displays = null;

            // Act & Assert
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => validator.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext));
        }

        /// <summary>
        /// Execution of ExecuteSheetAsyncFail_WithErrorMessageNullExceptionErrorWithNoRules should pass minimum information to ExecuteSheetAsync
        /// excluding Rules from FormStructure
        /// Expected result:
        /// NullExceptionError
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]

        public async Task ExecuteSheetAsyncFail_WithErrorMessageNullExceptionErrorWithNoRules()
        {
            // Arrange
            formStructure.Rules = null;

            // Act & Assert
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => validator.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext));
        }

        private async Task<List<string>> ReturnTestAsListAsync()
        {
            var result = await validator.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);
            var stringSeparators = new string[] { "\r\n" };
            var resultLines = result.Split(stringSeparators, StringSplitOptions.None)
                .Where(resultString => !string.IsNullOrWhiteSpace(resultString))
                .ToList();
            return resultLines;
        }

        private static FieldSheetValidator CreateSut()
        {
            return new FieldSheetValidator();
        }

        private ExcelSheetsData CreateSheetData()
        {
            var questionSet = CreateQuestionSet("1", "1", "RADIOGROUP", "1", "Main-Fields", "Section Rule", "1");
            return new ExcelSheetsData
            {
                Aggregations = new List<Aggregations>(),
                AnswerGuides = new List<AnswerGuide>(),
                Dependencies = new List<Dependencies>(),
                QuestionSet = new List<QuestionSet>
                {
                    questionSet
                }
            };
        }

        private static QuestionSet CreateQuestionSet(string questionSetNo, string fieldNo, string fieldType, string fieldDisplay, string sectionType, string sectionRule, string sectionNo)
        {
            var fields = new List<Field>
            {
                new Field { FieldNo = fieldNo, FieldType = fieldType, FieldDisplay = fieldDisplay }
            };
            var questionSet = new QuestionSet
            {
                QsNo = questionSetNo,
                Sections = new List<Section>
                {
                    new Section
                    {
                        Fields = fields,
                        SectionType = sectionType,
                        SectionRule = sectionRule,
                        SectionNo = sectionNo
                    }
                }
            };
            return questionSet;
        }

        private FormStructureData CreateFormStructure()
        {
            return new FormStructureData
            {
                SectionTypes = new List<SectionType>
                {
                    new SectionType
                    {
                        SectionTypeId = 1,
                        SectionTypes = "Main-Fields"
                    }
                },
                FieldTypes = new List<FieldType>
                {
                    new FieldType
                    {
                        FieldTypeId = 1,
                        FieldTypes = "RADIOGROUP"
                    }
                },
                Displays = new List<Display>
                {
                    new Display
                    {
                        DisplayId = 1,
                        Displays = "1"
                    }
                },
                Rules = new List<Rule>
                {
                    new Rule
                    {
                        RuleId = 1,
                        Rules = "Section Rule"
                    }
                }
            };
        }
    }
}
