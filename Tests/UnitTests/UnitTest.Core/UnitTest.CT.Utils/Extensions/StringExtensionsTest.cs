using CT.Utils.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace UnitTest.CT.Utils.Extensions
{
    /// <summary>
    /// String extensions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StringExtensionsTest
    {
        /// <summary>
        /// Test fir String stream
        /// </summary>
        [Fact]
        public void StringToStreamTest()
        {
            // Arrange
            var @string = "test string";

            // Act
            var stream = @string.ToStream();
            var streamcontent = stream.ToContentString();

            // Assert
            Assert.NotNull(stream);
            Assert.NotNull(streamcontent);
            Assert.Equal(streamcontent, @string);
        }

        /// <summary>
        /// Test for string to Enum
        /// </summary>
        [Fact]
        public void StringToEnumTest()
        {
            // Arrange
            var @string = "ABC";

            // Act
            var result = @string.ToEnum<TestString>();

            // Assert
            Assert.Equal(TestString.ABC, result);
        }

        /// <summary>
        /// Test for string formatting
        /// </summary>
        [Fact]
        public void StringToFormatTest()
        {
            // Arrange
            var param1 = 10;
            var param2 = "Hi";
            var param3 = DateTime.Now;
            var @string = "IntegerParam : {0},StringParam : {1},DateTimeParam : {2}";

            // Act
            var result = @string.ToFormat(param1, param2, param3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, $"IntegerParam : {param1},StringParam : {param2},DateTimeParam : {param3}");
        }

        /// <summary>
        /// Test to convert inbto Title Case
        /// </summary>
        [Fact]
        public void StringToTitleCaseTest()
        {
            // Arrange
            var @string = "testing string extension";

            // Act
            var result = @string.ToTitleCase();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Testing String Extension", result);
        }

        /// <summary>
        /// Test Enumeration
        /// </summary>
        public enum TestString
        {
            /// <summary>
            /// Value 1
            /// </summary>
            ABC,

            /// <summary>
            /// Value 2
            /// </summary>
            DEF,

            /// <summary>
            /// Value 3
            /// </summary>
            GHI
        }
    }
}