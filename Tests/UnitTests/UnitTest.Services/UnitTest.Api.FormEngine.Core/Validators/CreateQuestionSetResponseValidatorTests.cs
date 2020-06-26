using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Validators.Forms;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using System;
using TQ.Data.FormEngine;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Validators
{
    /// <summary>
    /// Unit tests for CreateQuestionSetResponseValidator
    /// </summary>
    public class CreateQuestionSetResponseValidatorTests
    {
        /// <summary>
        /// Validator returns an error when country is not specified in command
        /// </summary>
        /// <param name="country">country for the command</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ValidatorReturnsErrorWhenCountryIsNullOrEmpty(string country)
        {
            using (var formsEngineContext = new FormsEngineContext(GetDbContextOptions()))
            {
                // Arrange
                var sut = CreateSut(formsEngineContext);
                var command = new CreateQuestionSetResponse
                {
                    Country = country
                };

                // Act & Assert
                sut.ShouldHaveValidationErrorFor(command => command.Country, command);
            }
        }

        /// <summary>
        /// Validator returns an error when product is not specified in command
        /// </summary>
        /// <param name="product">product for the command</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ValidatorReturnsErrorWhenProductIsNullOrEmpty(string product)
        {
            using (var formsEngineContext = new FormsEngineContext(GetDbContextOptions()))
            {
                // Arrange
                var sut = CreateSut(formsEngineContext);
                var command = new CreateQuestionSetResponse
                {
                    Product = product
                };

                // Act & Assert
                sut.ShouldHaveValidationErrorFor(command => command.Product, command);
            }
        }

        /// <summary>
        /// Validator returns an error when ApplicationName is not specified in command
        /// </summary>
        /// <param name="applicationName">applicationName for the command</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ValidatorReturnsErrorWhenApplicationNameIsNullOrEmpty(string applicationName)
        {
            using (var formsEngineContext = new FormsEngineContext(GetDbContextOptions()))
            {
                // Arrange
                var sut = CreateSut(formsEngineContext);
                var command = new CreateQuestionSetResponse
                {
                    ApplicationName = applicationName
                };

                // Act & Assert
                sut.ShouldHaveValidationErrorFor(command => command.ApplicationName, command);
            }
        }

        /// <summary>
        /// Validator returns an error when Response is empty
        /// </summary>
        /// <param name="response">response for the command</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ValidatorReturnsErrorWhenResponseIsNullOrEmpty(string response)
        {
            using (var formsEngineContext = new FormsEngineContext(GetDbContextOptions()))
            {
                // Arrange
                var sut = CreateSut(formsEngineContext);
                var command = new CreateQuestionSetResponse
                {
                    Response = response
                };

                // Act & Assert
                sut.ShouldHaveValidationErrorFor(command => command.Response, command);
            }
        }

        /// <summary>
        /// Validator returns an error when QuestionSetId is not valid
        /// </summary>
        [Fact]
        public void ValidatorReturnsErrorWhenQuestionSetIdNotValid()
        {
            using (var formsEngineContext = new FormsEngineContext(GetDbContextOptions()))
            {
                // Arrange
                var sut = CreateSut(formsEngineContext);
                var command = new CreateQuestionSetResponse
                {
                    QuestionSetId = Guid.NewGuid()
                };

                // Act & Assert
                sut.ShouldHaveValidationErrorFor(command => command.QuestionSetId, command);
            }
        }


        /// <summary>
        /// Validator does not return any error when command is valid
        /// </summary>
        [Fact]
        public void ValidatorDoesNotReturnErrorWhenCommandIsValid()
        {
            using (var formsEngineContext = new FormsEngineContext(GetDbContextOptions()))
            {
                // Arrange
                var qs = formsEngineContext.AddQS(1);
                formsEngineContext.SaveChanges();

                var sut = CreateSut(formsEngineContext);
                var command = new CreateQuestionSetResponse
                {
                    Country = "TestCountry",
                    Product = "TestProduct",
                    ApplicationName = "TestApplication",
                    Response = "TestResponse",
                    QuestionSetId = qs.QSId
                };

                // Act
                var result = sut.TestValidate(command);

                // Assert
                result.IsValid.Should().BeTrue();
            }
        }

        private static DbContextOptions<FormsEngineContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<FormsEngineContext>()
                            .UseInMemoryDatabase(databaseName: "FormsEngineDb")
                            .Options;
        }

        private static CreateQuestionSetResponseValidator CreateSut(FormsEngineContext formsEngineContext)
        {
            return new CreateQuestionSetResponseValidator(formsEngineContext);
        }
    }
}