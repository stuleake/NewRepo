using Api.FormEngine.Core.SheetValidators;
using Api.FormEngine.Core.ViewModels.SheetModels;
using FluentAssertions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Data.FormEngine;
using UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders;
using Xunit;
using AnswerGuide = Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators
{
    /// <summary>
    /// Dependencies Sheet Validator Tests
    /// </summary>
    public class DependenciesSheetValidatorTests
    {
        /// <summary>
        /// Test for ExecuteSheetAsync to test the successful path.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_Returns_EmptyErrorString()
        {
            var sut = CreateSut();
            var dependencies = new DependencyBuilder()
                .Build();

            var sheetData = CreateSheetData(dependencies, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, CreateFormStructure(), Mock.Create<FormsEngineContext>());
            result.Should().BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for empty Dependencies properties.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForEmptyDependenciesItems_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var dependencies = new DependencyBuilder()
                .WithDecidesSection(null)
                .WithDependsCount(null)
                .WithDependsOnAns(null)
                .WithDependsOnAnsfromQS(null)
                .WithFieldNo(null)
                .WithSectionNo(null)
                .Build();

            var sheetData = CreateSheetData(dependencies, null);

            var result = await sut.ExecuteSheetAsync(sheetData, CreateFormStructure(), Mock.Create<FormsEngineContext>());
            result.Should().NotBeEmpty();
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for for Field Display Options not matching.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForFieldDisplayOptions_NotMatching_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var dependencies = new DependencyBuilder()
                .WithFieldDisplay("2")
                .Build();

            var sheetData = CreateSheetData(dependencies, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, CreateFormStructure(), Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Dependencies: Invalid Field Display : 2 in Field Number : 1");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for for Answer Guides not matching.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForAnswerGuides_NotMatching_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var dependencies = new DependencyBuilder()
                .WithDependsOnAnsfromQS(null)
                .Build();

            var answerGuide = new AnswerGuidesBuilder()
                .WithAnsNo("2")
                .Build();

            var sheetData = CreateSheetData(dependencies, CreateDefaultQuestionSet(), answerGuide);

            var result = await sut.ExecuteSheetAsync(sheetData, CreateFormStructure(), Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Dependencies: Depends On Ans : 1 does not exist in Answer Guide Sheet");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for for FieldNo not matching in QuestionsSet not matching.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForFieldNo_NotMatching_InQuestionSet_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var dependencies = new DependencyBuilder()
                .WithFieldNo("2")
                .Build();

            var sheetData = CreateSheetData(dependencies, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, CreateFormStructure(), Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Dependencies: Field No : 2 does not exist in Question Set Sheet");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for for SectionNo not matching in QuestionsSet not matching.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForSectionNo_NotMatching_InQuestionSet_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var dependencies = new DependencyBuilder()
                .WithFieldNo(null)
                .WithSectionNo("3")
                .Build();

            var sheetData = CreateSheetData(dependencies, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, CreateFormStructure(), Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Dependencies:Section No : 3 does not exist in Question Set Sheet");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for FieldNo and SectionNo empty.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForFieldNo_And_SectionNo_Empty_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var dependencies = new DependencyBuilder()
                .WithFieldNo(null)
                .WithSectionNo(null)
                .Build();

            var sheetData = CreateSheetData(dependencies, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, CreateFormStructure(), Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Dependencies: Both field and section cannot be empty");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for DecidesSectionNo not matching SectionNo in QuestionSet.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForDecidesSectionNo_NotMatching_SectionNo_InQuestionSet_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var dependencies = new DependencyBuilder()
                .WithDecidesSection("3")
                .Build();

            var sheetData = CreateSheetData(dependencies, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, CreateFormStructure(), Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Dependencies: Decides section : 3 does not exist in Question Set Sheet");
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

        private static QuestionSet CreateDefaultQuestionSet()
        {
            var section = new SectionBuilder()
                .WithField(new FieldBuilder().Build())
                .Build();

            return new QuestionSetBuilder()
                .WithSection(section)
                .Build();
        }

        private static FormStructureData CreateFormStructure()
        {
            return new FormStructureDataBuilder().Build();
        }

        private static DependenciesSheetValidator CreateSut()
        {
            return new DependenciesSheetValidator();
        }
    }
}
