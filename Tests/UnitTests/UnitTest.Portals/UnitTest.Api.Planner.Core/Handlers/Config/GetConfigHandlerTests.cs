using Api.Admin.Core.Commands.Config;
using Api.Admin.Core.Handlers.Config;
using CT.KeyVault;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using Xunit;

namespace UnitTest.Api.Planner.Core.Handlers.Config
{
    [ExcludeFromCodeCoverage]
    public class GetConfigHandlerTests
    {
        private readonly IVaultManager mockVaultManager = Mock.Create<IVaultManager>();
        private readonly CancellationToken cancellationToken = CancellationToken.None;
        private readonly GetConfigHandler sut;

        public GetConfigHandlerTests()
        {
            this.sut = new GetConfigHandler(mockVaultManager);
        }

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        [Fact]
        public async Task HandleSuccess()
        {
            // Arrange
            var keys = new List<string> { "key-1" };

            var request = new GetConfig
            {
                Keys = keys
            };

            var secret = "secret-1";
            Mock.Arrange(() => mockVaultManager.GetSecretAsync(Arg.AnyString)).Returns(Task.FromResult(secret)).MustBeCalled();

            var expected = new Dictionary<string, string> { { keys[0].Replace("-", "_", StringComparison.InvariantCulture), secret } };

            // Act
            var actual = await sut.Handle(request, this.cancellationToken);

            // Assert
            actual.Secrets.Should().BeEquivalentTo(expected);
            Mock.Assert(() => mockVaultManager.GetSecretAsync(Arg.AnyString), Occurs.Once());
        }
    }
}