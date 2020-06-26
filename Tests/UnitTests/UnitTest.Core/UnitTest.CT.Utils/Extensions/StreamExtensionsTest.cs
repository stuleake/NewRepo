using CT.Utils.Extensions;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace UnitTest.CT.Utils.Extensions
{
    [ExcludeFromCodeCoverage]
    public class StreamExtensionsTest
    {
        [Fact]
        public void StreamToContentStringTest()
        {
            // Arrange
            var testString = "test message";
            var stream = testString.ToStream();

            // Act
            var result = stream.ToContentString();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testString, result);
        }
    }
}