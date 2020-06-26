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
using UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders;
using Xunit;
using AnswerGuide = Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide;
using Field = Api.FormEngine.Core.ViewModels.SheetModels.Field;
using Section = Api.FormEngine.Core.ViewModels.SheetModels.Section;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators
{
    /// <summary>
    /// Answer Sheet Validation Tests
    /// </summary>
    public class AnswerGuideSheetValidatorTests
    {
        private readonly FormStructureData formStructure;
        private readonly AnswerGuideSheetValidator sut;
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerGuideSheetValidatorTests"/> class.
        /// </summary>
        public AnswerGuideSheetValidatorTests()
        {
            sut = CreateSut();
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
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");
            var sheetData = CreateSheetData(dependencies, questionSet);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>()
                .And.BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailWithErrorFieldNoDoesNotExit should pass
        /// minimum information to ExecuteSheetAsync
        /// Expected Result :
        /// Field No : x does not exist in Question Set Sheet
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailWithErrorFieldNoDoesNotExist()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithFieldNo("8")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var resultLines = await ReturnTestAsListAsync(sheetData);

            // Assert
            resultLines.Should().BeOfType<List<string>>()
            .And.NotBeEmpty()
            .And.HaveCount(1);
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailWithErrorFieldNoDoesNotExit should pass
        /// minimum information to ExecuteSheetAsync
        /// Expected Result :
        /// Invalid Range Minimum Number : one
        /// Invalid Range Maximum Number : ten
        /// Can't set range min, range max, date min, date max, multiple if field type is Text. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailWithErrorInvalidRanges()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "NUMBER", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinRange("one")
                .WithMaxRange("ten")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var resultLines = await ReturnTestAsListAsync(sheetData);

            // Assert
            resultLines.Should().BeOfType<List<string>>();
            resultLines.Should().NotBeEmpty();
            resultLines.Should().HaveCount(2);
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailWithErrorInvalidLengths should pass
        /// minimum information to ExecuteSheetAsync
        /// Expected Result :
        /// Invalid Length Minimum Number : one
        /// Invalid Length Maximum Number : ten
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailWithErrorInvalidLengths()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "NUMBER", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinLength("one")
                .WithMaxLength("ten")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var resultLines = await ReturnTestAsListAsync(sheetData);

            // Assert
            resultLines.Should().BeOfType<List<string>>()
            .And.NotBeEmpty()
            .And.HaveCount(2);
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailWithErrorInvalidLengths should pass
        /// minimum information to ExecuteSheetAsync
        /// Expected Result :
        /// Invalid Length Minimum Number : one
        /// Invalid Length Maximum Number : ten
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailWithErrorInvalidLength_MinGreaterThanMax()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinLength("10")
                .WithMaxLength("1")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var resultLines = await ReturnTestAsListAsync(sheetData);

            // Assert
            resultLines.Should().BeOfType<List<string>>()
            .And.NotBeEmpty()
            .And.HaveCount(1);
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailWithErrorInvalidMultiple should pass
        /// minimum information to ExecuteSheetAsync
        /// Expected Result :
        /// Invalid Multiple Number : x1
        /// Can't set range min, range max, date min, date max, multiple if field type is Text. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailWithErrorInvalidMultiple()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "NUMBER", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMultiple("x1")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var resultLines = await ReturnTestAsListAsync(sheetData);

            // Assert
            resultLines.Should().BeOfType<List<string>>()
            .And.NotBeEmpty()
            .And.HaveCount(1);
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailWithErrorInvalidDefault_True should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of true
        /// Expected Result :
        /// Value Tru is not boolean in IsDefault column
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailWithErrorInvalidDefault_True()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithIsDefault("Tru")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var resultLines = await ReturnTestAsListAsync(sheetData);

            // Assert
            resultLines.Should().BeOfType<List<string>>()
            .And.NotBeEmpty()
            .And.HaveCount(1);
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailWithErrorInvalidDefault_False should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Value fales is not boolean in IsDefault column
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailWithErrorInvalidDefault_False()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithIsDefault("fales")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var resultLines = await ReturnTestAsListAsync(sheetData);

            // Assert
            resultLines.Should().BeOfType<List<string>>()
            .And.NotBeEmpty()
            .And.HaveCount(1);
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_SuccessMinDateRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Pass the test and return an empty string
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_SuccessMinDateRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinLength(string.Empty)
                .WithMaxLength(string.Empty)
                .WithMinDate("today")
                .WithMaxDate(string.Empty)
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_SuccessMaxDateRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Pass the test and return an empty string
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_SuccessMaxDateRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinLength(string.Empty)
                .WithMaxLength(string.Empty)
                .WithMinDate(string.Empty)
                .WithMaxDate("today")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_SuccessMinAndMaxDateRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Pass the test and return an empty string
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_SuccessMinAndMaxDateRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinDate("today")
                .WithMaxDate("today")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailDateRangesWithLength should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, multiple, api if field type is Date. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailDateRangesWithLength()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinLength("4")
                .WithMaxLength("40")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailDateRangesWithMultiple should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, multiple, api if field type is Date. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailDateRangesWithMultiple()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMultiple("4")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailDateRangesWithAPI should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, multiple, api if field type is Date. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailDateRangesWithAPI()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithApi("/api/FakePath")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailDateRangesWithLength should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, multiple, api if field type is Date. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailTextRangesWithRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "TEXT", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinRange("1")
                .WithMaxRange("10")
                .WithMinDate("today")
                .WithMaxDate(string.Empty)
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailTextWithRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, date min, date max, multiple if field type is Text. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailDateRangeWithRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DATE", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinRange("1")
                .WithMaxRange("10")
                .WithMinDate("today")
                .WithMaxDate(string.Empty)
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_SuccessText should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Pass the test and return an empty string
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_SuccessText()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "TEXT", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinLength("1")
                .WithMaxLength("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailTextWithDateRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, date min, date max, multiple if field type is Text. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailTextWithDateRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "TEXT", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinDate("1")
                .WithMaxDate("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailTextWithRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, date min, date max, multiple if field type is Text. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailTextWithRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "TEXT", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinRange("1")
                .WithMaxRange("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailTextWithRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, date min, date max, multiple if field type is Text. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailTextWithMultiple()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "TEXT", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMaxRange("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailDateRangesWithAPI should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, multiple, api if field type is Number. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailNumberWithAPI()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "NUMBER", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithApi("/api/FakePath")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailNumberWithDateRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, multiple, api if field type is Number. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailNumberWithDateRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "NUMBER", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinDate("today")
                .WithMaxDate("today")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_SuccessNumberWithLength should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Should Pass and return an empty string
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_SuccessNumberWithLength()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "NUMBER", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinLength("1")
                .WithMaxLength("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_SuccessNumberWithRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Should pass and return an empty string
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_SuccessNumberWithRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "NUMBER", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinRange("1")
                .WithMaxRange("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_SuccessDropdown should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Should pass and return an empty string
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_SuccessDropdown()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DROPDOWN", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailDropdownWithRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, date min, date max, multiple if field type is Dropdown/Radio. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailDropdownWithRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DROPDOWN", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinRange("1")
                .WithMaxRange("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailDropdownWithLength should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, date min, date max, multiple if field type is Dropdown/Radio. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailDropdownWithLength()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DROPDOWN", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinLength("1")
                .WithMaxLength("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailDropdownWithDateRange should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, date min, date max, multiple if field type is Dropdown/Radio. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailDropdownWithDateRange()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DROPDOWN", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMinDate("1")
                .WithMaxDate("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Execution of ExecuteSheetAsync_FailDropdownWithMultiple should pass
        /// minimum information to ExecuteSheetAsync
        /// Passing an incorrectly spelled version of false
        /// Expected Result :
        /// Can't set range min, range max, length min, length max, date min, date max, multiple if field type is Dropdown/Radio. Field Number : 7
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_FailDropdownWithMultiple()
        {
            // Arrange
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var questionSet = CreateDefaultQuestionSet("7", "DROPDOWN", "10", "Main-Fields", "Section Rule", "1");

            var answerGuide = new AnswerGuidesBuilder()
                .WithMultiple("10")
                .Build();

            var sheetData = CreateSheetData(dependencies, questionSet, answerGuide);

            // Act
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);

            // Assert
            result.Should().BeOfType<string>().And.NotBeNullOrWhiteSpace();
        }

        private async Task<List<string>> ReturnTestAsListAsync(ExcelSheetsData sheetData)
        {
            var result = await sut.ExecuteSheetAsync(sheetData, formStructure, formsEngineContext);
            var stringSeparators = new string[] { "\r\n" };
            var resultLines = result.Split(stringSeparators, StringSplitOptions.None)
                .Where(resultString => !string.IsNullOrWhiteSpace(resultString))
                .ToList();
            return resultLines;
        }

        private static AnswerGuideSheetValidator CreateSut()
        {
            return new AnswerGuideSheetValidator();
        }

        private static ExcelSheetsData CreateSheetData(Dependencies dependencies, QuestionSet questionSet, AnswerGuide answerGuide = null)
        {
            var builder = new ExcelSheetsDataBuilder()
                .WithDependency(dependencies)
                .WithQuestionSet(questionSet);

            if (answerGuide != null)
            {
                builder.WithAnswerGuide(answerGuide);
            }

            return builder.Build();
        }

        private static QuestionSet CreateDefaultQuestionSet(string fieldNo, string fieldType, string fieldDisplay, string sectionType, string sectionRule, string sectionNo)
        {
            var fields = new List<Field>
            {
                new Field
                {
                    FieldNo = fieldNo,
                    FieldType = fieldType,
                    FieldDisplay = fieldDisplay
                }
            };
            var section = new List<Section>
            {
                new Section
                {
                    Fields = fields,
                    SectionType = sectionType,
                    SectionRule = sectionRule,
                    SectionNo = sectionNo
                }
            };

            return new QuestionSetBuilder()
                .WithSections(section)
                .Build();
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
                        FieldTypes = "TEXT"
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