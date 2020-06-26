using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Api.FormEngine.Core.Services.Validators;
using Api.FormEngine.Core.ViewModels.SheetModels;
using Microsoft.Extensions.DependencyInjection;
using TQ.Data.FormEngine;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators
{
    /// <summary>
    /// QuestionSet Validator Tests
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class QuestionSetValidatorTests
    {
        private readonly FormsEngineContext mockFormsEngineContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FormsEngineContext>();

        /// <summary>
        /// Test to assert a sheet successfully validates.
        /// </summary>
        [Fact]
        public void ValidSheetSuccessfullyValidates()
        {
            // Arrange
            var validator = CreateSut();
            var sheet = new ExcelSheetsData
            {
                Aggregations = new List<Aggregations>(),
                AnswerGuides = new List<AnswerGuide>(),
                Dependencies = new List<Dependencies>(),
                QuestionSet = new List<QuestionSet> { new QuestionSet { QsNo = "0", Sections = new List<Section> { new Section { Fields = new List<Field> { new Field { FieldNo = "0" } } } } } }
            };

            // Act
            var result = validator.Validate(sheet);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.Null(result.Messages);
        }

        /// <summary>
        /// Test to Assert an invalid sheet throws SystemException.
        /// </summary>
        [Fact]
        public void InvalidSheetThrows()
        {
            // Arrange
            var validator = CreateSut();
            var sheet = new ExcelSheetsData
            {
                Aggregations = new List<Aggregations>(),
                AnswerGuides = new List<AnswerGuide>(),
                Dependencies = new List<Dependencies>(),
                QuestionSet = new List<QuestionSet>()
            };

            // Act & Assert
            Assert.ThrowsAny<SystemException>(() => validator.Validate(sheet));
        }

        private QuestionSetValidator CreateSut()
        {
            return new QuestionSetValidator(mockFormsEngineContext);
        }
    }
}