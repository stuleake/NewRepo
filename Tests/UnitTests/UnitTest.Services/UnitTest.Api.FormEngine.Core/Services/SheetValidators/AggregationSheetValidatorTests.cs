using Api.FormEngine.Core.SheetValidation;
using Api.FormEngine.Core.ViewModels.SheetModels;
using FluentAssertions;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Data.FormEngine;
using UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators
{
    /// <summary>
    /// Aggregation Sheet Validator Tests
    /// </summary>
    public class AggregationSheetValidatorTests
    {
        /// <summary>
        /// Test for ExecuteSheetAsync to test the successful path.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_Returns_EmptyErrorString()
        {
            var sut = CreateSut();
            var aggregations = new AggregationsBuilder().Build();
            var formStructureData = new FormStructureDataBuilder().Build();

            var sheetData = CreateSheetData(aggregations, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, formStructureData, Mock.Create<FormsEngineContext>());
            result.Should().BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for FieldType not a number.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForFieldTypeNotANumber_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var aggregations = new AggregationsBuilder().Build();
            var formStructureData = new FormStructureDataBuilder().Build();
            var section = new SectionBuilder()
                .WithField(new FieldBuilder()
                    .WithFieldNo("1")
                    .WithFieldType("xyz")
                    .Build())
                .Build();

            var questionSet = new QuestionSetBuilder()
                .WithSection(section)
                .Build();
            var sheetData = CreateSheetData(aggregations, questionSet);

            var result = await sut.ExecuteSheetAsync(sheetData, formStructureData, Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Field type : xyz is not Number, cannot process this aggregation. Field Number : 1");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for FieldNo not in QuestionSet.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForFieldNo_NotExistInQuestionSet_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var aggregations = new AggregationsBuilder().Build();
            var formStructureData = new FormStructureDataBuilder().Build();
            var section = new SectionBuilder()
                .WithField(new FieldBuilder()
                    .WithFieldNo("3")
                    .WithFieldType("number")
                    .Build())
                .Build();

            var questionSet = new QuestionSetBuilder()
                .WithSection(section)
                .Build();
            var sheetData = CreateSheetData(aggregations, questionSet);

            var result = await sut.ExecuteSheetAsync(sheetData, formStructureData, Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Field No : 1 does not exist in Question Set SheetCannot process this aggregation");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for invalid FieldNo.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForFieldNo_Invalid_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var aggregations = new AggregationsBuilder()
                .WithFieldNo("xyz")
                .Build();
            var formStructureData = new FormStructureDataBuilder().Build();
            var sheetData = CreateSheetData(aggregations, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, formStructureData, Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Invalid Field No Number : xyz Cannot process this aggregation");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for FieldNo and AggregatedFieldNo are the same.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForFieldNo_AndAggregatedFieldNo_AreTheSame_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var aggregations = new AggregationsBuilder()
                .WithAggregatedFieldNo("1")
                .Build();
            var formStructureData = new FormStructureDataBuilder().Build();
            var sheetData = CreateSheetData(aggregations, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, formStructureData, Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Field Number 1 and aggregation number 1 both are same cannot process this aggregation");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for duplicate Aggregations.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForDuplicateAggregations_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var aggregations1 = new AggregationsBuilder()
                .Build();
            var aggregations2 = new AggregationsBuilder()
                .WithAggregatedFieldNo("3")
                .Build();

            var formStructureData = new FormStructureDataBuilder().Build();
            var sheetData = CreateSheetData(aggregations1, CreateDefaultQuestionSet());
            sheetData.Aggregations.Add(aggregations2);

            var result = await sut.ExecuteSheetAsync(sheetData, formStructureData, Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Duplicate priority number found in field number : 1 cannot process this aggregation");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for FieldNo not existing.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForAggregatedFieldNo_DoesNotExist_InQuestionSet_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var aggregations = new AggregationsBuilder()
                .WithAggregatedFieldNo(null)
                .Build();
            var formStructureData = new FormStructureDataBuilder().Build();
            var sheetData = CreateSheetData(aggregations, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, formStructureData, Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Aggregation Field Number :  does not exist in Question Set Sheet");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for invalid function.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForInvalidFunction_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var aggregations = new AggregationsBuilder()
                .WithFunction("xyz")
                .Build();
            var formStructureData = new FormStructureDataBuilder().Build();
            var sheetData = CreateSheetData(aggregations, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, formStructureData, Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Invalid function : xyz");
        }

        /// <summary>
        /// Test for ExecuteSheetAsync to test that an error message string is returned for invalid priority.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task ExecuteSheetAsync_ForInvalidPriority_Returns_ErrorMessageString()
        {
            var sut = CreateSut();
            var aggregations = new AggregationsBuilder()
                .WithPriority("xyz")
                .Build();
            var formStructureData = new FormStructureDataBuilder().Build();
            var sheetData = CreateSheetData(aggregations, CreateDefaultQuestionSet());

            var result = await sut.ExecuteSheetAsync(sheetData, formStructureData, Mock.Create<FormsEngineContext>());
            result.Should()
                .NotBeEmpty()
                .And
                .Contain("Invalid Priority Number : xyz");
        }

        private static ExcelSheetsData CreateSheetData(Aggregations aggregations, QuestionSet questionSet)
        {
            return new ExcelSheetsDataBuilder()
                .WithAggregations(aggregations)
                .WithQuestionSet(questionSet)
                .Build();
        }

        private static QuestionSet CreateDefaultQuestionSet()
        {
            var section = new SectionBuilder()
                .WithField(new FieldBuilder()
                    .WithFieldNo("1")
                    .WithFieldType("number")
                    .Build())
                .WithField(new FieldBuilder()
                    .WithFieldNo("2")
                    .WithFieldType("number")
                    .Build())
                .Build();

            return new QuestionSetBuilder()
                .WithSection(section)
                .Build();
        }

        private static AggregationSheetValidator CreateSut()
        {
            return new AggregationSheetValidator();
        }
    }
}
