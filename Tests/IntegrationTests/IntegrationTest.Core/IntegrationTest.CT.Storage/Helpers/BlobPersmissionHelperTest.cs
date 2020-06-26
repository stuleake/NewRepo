using CT.Storage.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Test.CT.Storage.Helpers
{
    [ExcludeFromCodeCoverage]
    public class BlobPersmissionHelperTest
    {
        [Fact]
        public void GetPersmissionsTest()
        {
            // Arrange
            var permissions = "none|read|write|list|delete|create|add";

            // Act
            var result = BlobPersmissionHelper.GetPersmissions(permissions);

            // Assert
            Assert.Equal(typeof(SharedAccessBlobPermissions), result.GetType());
            Assert.Equal(
                SharedAccessBlobPermissions.Read |
                SharedAccessBlobPermissions.Write |
                SharedAccessBlobPermissions.Delete |
                SharedAccessBlobPermissions.List |
                SharedAccessBlobPermissions.Add |
                SharedAccessBlobPermissions.Create, result);
        }

        [Fact]
        public void GetInvalidPermissionsTest()
        {
            // Arrange
            var permissions = "invalid-permissions";
            Exception error = null;

            // Act
            try
            {
                _ = BlobPersmissionHelper.GetPersmissions(permissions);
            }
            catch (NotSupportedException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
            Assert.True(string.Compare(error.Message, "Permission Not Supported", true) == 0);
        }

        /// <summary>
        /// Integration test case for null validation of method GetPersmissionsNullTest
        /// </summary>
        [Fact]
        public void GetPersmissionsNullTest()
        {
            // Act
            Exception error = null;
            try
            {
                _ = BlobPersmissionHelper.GetPersmissions(null);
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