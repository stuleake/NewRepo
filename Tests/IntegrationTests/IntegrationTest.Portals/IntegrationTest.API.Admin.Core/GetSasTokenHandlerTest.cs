using Api.Admin.Core.Commands.Blob;
using Api.Admin.Core.Handlers.Blob;
using CT.Storage;
using CT.Storage.Exceptions;
using IntegrationTest.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.Api.Admin.Core
{
    /// <summary>
    /// class to test Get SasToken handler
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetSasTokenHandlerTest
    {
        private readonly ServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSasTokenHandlerTest"/> class.
        /// </summary>
        public GetSasTokenHandlerTest()
        {
            serviceProvider = IntegrationHelper.GiveServiceProvider();
        }

        /// <summary>
        /// Get sas token Handler Test
        /// </summary>
        /// <param name="containerName">containerName</param>
        /// <param name="fileName">fileName</param>
        /// <returns>true is success</returns>
        [Theory]
        [InlineData("documents", "")]
        public async Task Handle_GetSasToken(string containerName, string fileName)
        {
            var cmd = new GetSasToken { ContainerName = containerName, FileName = fileName, Permissions = "read | write" };

            var handler = new GetSasTokenHandler(serviceProvider.GetRequiredService<IStorageManager>());

            var actual = await handler.Handle(cmd, System.Threading.CancellationToken.None);
            Assert.NotNull(actual);
        }

        /// <summary>
        /// Get sas token Handler Exception Test
        /// </summary>
        /// <param name="containerName">containerName</param>
        /// <param name="fileName">fileName</param>
        /// <returns>true is success</returns>
        [Theory]
        [InlineData("document", "")]
        public async Task Handle_GetSasToken_Exception(string containerName, string fileName)
        {
            var cmd = new GetSasToken { ContainerName = containerName, FileName = fileName, Permissions = "read | write" };

            var handler = new GetSasTokenHandler(serviceProvider.GetRequiredService<IStorageManager>());

            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(cmd, System.Threading.CancellationToken.None));
        }
    }
}