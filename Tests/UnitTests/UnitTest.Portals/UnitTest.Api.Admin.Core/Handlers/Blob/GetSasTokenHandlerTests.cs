using Api.Admin.Core.Commands.Blob;
using Api.Admin.Core.Handlers.Blob;
using CT.Storage;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.Blob
{
    [ExcludeFromCodeCoverage]
    public class GetSasTokenHandlerTests
    {
        private readonly IStorageManager mockStorageManager = Mock.Create<IStorageManager>();
        private readonly System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
        private readonly GetSasTokenHandler sut;

        public GetSasTokenHandlerTests()
        {
            this.sut = new GetSasTokenHandler(mockStorageManager);
        }

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
           // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        [Fact]
        public async Task HandleSuccess()
        {
            // Arrange
           var request = new GetSasToken
            {
                ContainerName = "Container Name",
                FileName = "File Name",
                Permissions = "Permissions"
            };

            string expected = "expected token";
            Mock.Arrange(() => mockStorageManager.GetSasTokenAsync(request.ContainerName, request.FileName, request.Permissions)).Returns(Task.FromResult(expected)).MustBeCalled();

            // Act
            var actual = await sut.Handle(request, this.cancellationToken);

            // Assert
            actual.Should().Be(expected);
            Mock.Assert(() => mockStorageManager.GetSasTokenAsync(request.ContainerName, request.FileName, request.Permissions), Occurs.Once());
        }
    }
}
