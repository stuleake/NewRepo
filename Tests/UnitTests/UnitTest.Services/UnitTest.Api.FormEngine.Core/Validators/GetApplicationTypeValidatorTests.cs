using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Validators.Forms;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Validators
{
    /// <summary>
    /// Unit tests for GetApplicationTypeValidator
    /// </summary>
    public class GetApplicationTypeValidatorTests
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
            // Arrange
            var sut = CreateSut();
            var command = new GetApplicationType
            {
                Country = country
            };

            // Act & Assert
            sut.ShouldHaveValidationErrorFor(command => command.Country, command);
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
            // Arrange
            var sut = CreateSut();
            var command = new GetApplicationType
            {
                Product = product
            };

            // Act & Assert
            sut.ShouldHaveValidationErrorFor(command => command.Product, command);
        }

        /// <summary>
        /// Validator does not return an error when command is valid
        /// </summary>
        [Fact]
        public void ValidatorDoesNotReturnErrorWhenCommandIsValid()
        {
            // Arrange
            var sut = CreateSut();
            var command = new GetApplicationType
            {
                Country = "TestCountry",
                Product = "TestCProduct"
            };

            // Act
            var result = sut.TestValidate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }


        private GetApplicationTypeValidator CreateSut()
        {
            return new GetApplicationTypeValidator();
        }
    }
}