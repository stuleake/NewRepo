using Api.FormEngine.Core.SheetValidators;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators
{
    /// <summary>
    /// Sheet Validator Tests
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SheetValidatorTests
    {
        /// <summary>
        /// Test for CheckNumber with Type parameter method returns expected result.
        /// </summary>
        /// <param name="input">The input to test.</param>
        /// <param name="type">The type value.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData("1", "", "")]
        [InlineData("", "numberValue", "numberValue cannot be null or empty")]
        [InlineData("xyz", "numberValue", "Invalid numberValue Number : xyz")]
        public void CheckNumber_WithType_Returns_ExpectedResult(string input, string type, string expected)
        {
            var result = SheetValidator.CheckIsNumber(input, type);
            result.Should().Be(expected);
        }

        /// <summary>
        /// Test for CheckNumber method returns expected result.
        /// </summary>
        /// <param name="input">The input to test.</param>
        /// <param name="expected">The expected result</param>
        [Theory]
        [InlineData("1", true)]
        [InlineData("xyz", false)]
        public void CheckNumber_Returns_ExpectedResult(string input, bool expected)
        {
            var result = SheetValidator.CheckIsNumber(input);
            result.Should().Be(expected);
        }

        /// <summary>
        /// Test for CheckValueIsNumberAndNotEmpty with Message parameter methods returns expected result.
        /// </summary>
        /// <param name="input">The input to test.</param>
        /// <param name="message">The message value.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData("1", "", "")]
        [InlineData("", "", "")]
        [InlineData("xyz", "messageValue", "Invalid messageValue Number : xyz")]
        public void CheckValueIsNumberAndNotEmpty_WithMessage_Returns_ExpectedResult(string input, string message, string expected)
        {
            var result = SheetValidator.CheckValueIsNumberAndNotEmpty(input, message);
            result.Should().Be(expected);
        }

        /// <summary>
        /// Test for CheckValueIsNumberAndNotEmpty methods returns expected result.
        /// </summary>
        /// <param name="input">The input to test.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData("1", true)]
        [InlineData("", false)]
        [InlineData("xyz", false)]
        public void CheckValueIsNumberAndNotEmpty_Returns_ExpectedResult(string input, bool expected)
        {
            var result = SheetValidator.CheckValueIsNumberAndNotEmpty(input);
            result.Should().Be(expected);
        }

        /// <summary>
        /// Test for CheckIsNotNullOrEmpty methods returns expected result.
        /// </summary>
        /// <param name="input">The input to test.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        [InlineData("abc", true)]
        public void CheckIsNotNullOrEmpty_Returns_ExpectedResult(string input, bool expected)
        {
            var result = SheetValidator.CheckIsNotNullOrEmpty(input);
            result.Should().Be(expected);
        }

        /// <summary>
        /// Test for BoolErrorMessage with Message parameter methods returns expected result.
        /// </summary>
        /// <param name="input">The input to test.</param>
        /// <param name="message">The message value.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData("true", "", "")]
        [InlineData("false", "", "")]
        [InlineData("xyz", "messageValue", "Value xyz is not boolean in messageValue column")]
        public void BoolErrorMessage_Returns_ExpectedResult(string input, string message, string expected)
        {
            var result = SheetValidator.BoolErrorMessage(input, message);
            result.Should().Be(expected);
        }

        /// <summary>
        /// Test for CheckValueIsBool methods returns expected result.
        /// </summary>
        /// <param name="input">The input to test.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData("true", true)]
        [InlineData("false", true)]
        [InlineData("xyz", false)]
        public void CheckValueIsBool_Returns_ExpectedResult(string input, bool expected)
        {
            var result = SheetValidator.CheckValueIsBool(input);
            result.Should().Be(expected);
        }
    }
}
