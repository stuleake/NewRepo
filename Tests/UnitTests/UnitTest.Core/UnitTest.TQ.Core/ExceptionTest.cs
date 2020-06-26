using System.Diagnostics.CodeAnalysis;
using TQ.Core.Exceptions;
using Xunit;

namespace UnitTest.TQ.Core
{
    /// <summary>
    /// Class to test exceptions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExceptionTest
    {
        /// <summary>
        /// Test to check TQException
        /// </summary>
        [Fact]
        public void TQExceptionTest()
        {
            // Arrange
            var innerException = new TQException("InnerException");
            var mainException = new TQException("OuterException", innerException);

            // Assert
            Assert.NotNull(innerException);
            Assert.True(string.Compare(innerException.Message, "InnerException", true) == 0);
            Assert.NotNull(mainException);
            Assert.True(string.Compare(mainException.Message, "OuterException", true) == 0);
            Assert.NotNull(mainException.InnerException);
            Assert.True(string.Compare(mainException.InnerException.Message, "InnerException", true) == 0);
        }

        /// <summary>
        /// Test to check MappingResponseException
        /// </summary>
        [Fact]
        public void MappingResponseExceptionTest()
        {
            // Arrange
            var innerException = new MappingResponseException("InnerException");
            var mainException = new MappingResponseException("OuterException", innerException);

            // Assert
            Assert.NotNull(innerException);
            Assert.True(string.Compare(innerException.Message, "InnerException", true) == 0);
            Assert.NotNull(mainException);
            Assert.True(string.Compare(mainException.Message, "OuterException", true) == 0);
            Assert.NotNull(mainException.InnerException);
            Assert.True(string.Compare(mainException.InnerException.Message, "InnerException", true) == 0);
        }

        /// <summary>
        /// Test to check NoContentException
        /// </summary>
        [Fact]
        public void NoContentExceptionTest()
        {
            // Arrange
            var innerException = new NoContentException("InnerException");
            var mainException = new NoContentException("OuterException", innerException);

            // Assert
            Assert.NotNull(innerException);
            Assert.True(string.Compare(innerException.Message, "InnerException", true) == 0);
            Assert.NotNull(mainException);
            Assert.True(string.Compare(mainException.Message, "OuterException", true) == 0);
            Assert.NotNull(mainException.InnerException);
            Assert.True(string.Compare(mainException.InnerException.Message, "InnerException", true) == 0);
        }
    }
}