using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TQ.Core.Extensions;
using Xunit;

namespace UnitTest.TQ.Core.Extensions
{
    /// <summary>
    /// Unit tests fir the string class extension methods
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StringExtensionTests
    {
        /// <summary>
        /// Tests that the supplied postcode is valid
        /// </summary>
        /// <param name="postCode">the postcode to test</param>
        [Theory]
        [InlineData("EC1A 1BB")]
        [InlineData("EC1A1BB")]
        [InlineData("W1A 0AX")]
        [InlineData("W1A0AX")]
        [InlineData("M1 1AE")]
        [InlineData("M11AE")]
        [InlineData("B33 8TH")]
        [InlineData("B338TH")]
        [InlineData("CR2 6XH")]
        [InlineData("CR26XH")]
        [InlineData("DN55 1PT")]
        [InlineData("DN551PT")]
        public void IsPostCodeReturnsTrueWhenInputIsPostCode(string postCode)
        {
            // Arrange
            var sut = postCode;

            // Act
            var result = sut.IsPostCode();

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests that the supplied postcode is in-valid
        /// </summary>
        /// <param name="postCode">the postcode to test</param>
        [Theory]
        [InlineData("EC1A1B")]
        [InlineData("W1A_0AX")]
        [InlineData("_M1 1AE")]
        [InlineData("Hello World")]
        [InlineData("CR2 6XH!")]
        [InlineData("DN55 *PT")]
        public void IsPostCodeReturnsFalseWhenInputIsNotPostCode(string postCode)
        {
            // Arrange
            var sut = postCode;

            // Act
            var result = sut.IsPostCode();

            // Assert
            result.Should().BeFalse();
        }
    }
}