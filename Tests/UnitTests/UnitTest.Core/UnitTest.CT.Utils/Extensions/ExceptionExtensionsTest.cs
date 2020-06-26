using CT.Utils.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace UnitTest.CT.Utils.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ExceptionExtensionsTest
    {
        [Fact]
        public void FlattenTest()
        {
            // Arrange
            Exception exception = null;
            try
            {
                throw new Exception("test exception message");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Act
            var result = exception?.Flatten();

            // Assert
            Assert.NotNull(result);
            Assert.Equal($"Exception: {exception.Message}\nTrace:{exception.StackTrace}\n", result);
        }

        [Fact]
        public void FlattenWithInnerExceptionTest()
        {
            // Arrange
            Exception exception = null;
            try
            {
                try
                {
                    throw new Exception("inner exception message");
                }
                catch (Exception ex)
                {
                    throw new Exception("outer exception message", ex);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Act
            var result = exception?.Flatten();

            // Assert
            Assert.Equal($"Exception: {exception.Message}\nTrace:{exception.StackTrace}\n\nInner Exception: {exception.InnerException.Message}\nTrace:{exception.InnerException.StackTrace}\n", result);
        }

        /// <summary>
        /// Unit test case for null validation in Flatten method
        /// </summary>
        [Fact]
        public void FlattenNullTest()
        {
            // Act
            Exception error = null;
            try
            {
                _ = ExceptionExtensions.Flatten(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }
    }
}