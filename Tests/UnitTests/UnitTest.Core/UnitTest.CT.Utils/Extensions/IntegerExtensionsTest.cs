using CT.Utils.Extensions;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace UnitTest.CT.Utils.Extensions
{
    [ExcludeFromCodeCoverage]
    public class IntegerExtensionsTest
    {
        public enum Test
        {
            Zero = 0,
            One = 1,
            Two = 2
        }

        [Fact]
        public void ToEnumTest()
        {
            // Arrange
            int testNum = 0;

            // Act
            var result = testNum.ToEnum<Test>();

            // Assert
            Assert.Equal(Test.Zero, result);
        }
    }
}