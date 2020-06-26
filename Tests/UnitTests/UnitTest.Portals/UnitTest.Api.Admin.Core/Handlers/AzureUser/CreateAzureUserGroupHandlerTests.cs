using Api.Admin.Core.Commands.AzureUser;
using Api.Admin.Core.Handlers.AzureUser;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Helpers;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.AzureUser
{
    [ExcludeFromCodeCoverage]
    public class CreateAzureUserGroupHandlerTests
    {
        private readonly IB2CGraphClient mockB2CGraphClient = Mock.Create<IB2CGraphClient>();
        private readonly CancellationToken cancellationToken = new CancellationToken();
        private readonly CreateAzureUserGroupHandler sut;

        public CreateAzureUserGroupHandlerTests()
        {
            sut = new CreateAzureUserGroupHandler(mockB2CGraphClient);
        }

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        [Fact]
        public async Task HandleReturnsTrue()
        {
            // Arrange
            var request = new AzureUserGroupModel
            {
                Description = "test description",
                DisplayName = "display name",
                MailEnabled = true,
                MailNickname = "mail nick name",
                SecurityEnabled = true
            };

            string azureGroup = JsonConvert.SerializeObject(request);
            Mock.Arrange(() => mockB2CGraphClient.CreateGroupAsync(azureGroup)).Returns(Task.FromResult(true)).MustBeCalled();

            // Act
            var result = await sut.Handle(request, this.cancellationToken);

            // Assert
            result.Should().BeTrue();
            Mock.Assert(() => mockB2CGraphClient.CreateGroupAsync(azureGroup), Occurs.Once());
        }

        [Fact]
        public async Task HandleReturnsFalse()
        {
            // Arrange
            var request = new AzureUserGroupModel
            {
                Description = "test description",
                DisplayName = "display name",
                MailEnabled = true,
                MailNickname = "mail nick name",
                SecurityEnabled = true
            };

            string azureGroup = JsonConvert.SerializeObject(request);
            Mock.Arrange(() => mockB2CGraphClient.CreateGroupAsync(azureGroup)).Returns(Task.FromResult(false)).MustBeCalled();

            // Act
            var result = await sut.Handle(request, this.cancellationToken);

            // Assert
            result.Should().BeFalse();
            Mock.Assert(() => mockB2CGraphClient.CreateGroupAsync(azureGroup), Occurs.Once());
        }
    }
}
