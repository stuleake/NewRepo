using Api.Admin.Core.Commands.AzureUser;
using Api.Admin.Core.Handlers.AzureUser;
using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Helpers;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.AzureUser
{
    [ExcludeFromCodeCoverage]
    public class AzureAssignUsertoGroupTests
    {
        private readonly IB2CGraphClient mockB2CGraphClient = Mock.Create<IB2CGraphClient>();
        private readonly System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
        private readonly AzureAssignUsertoGroupHandler sut;

        private const string GroupObjectId = "GroupObjectid";
        private const string UserObjectId = "UserObjectid";

        public AzureAssignUsertoGroupTests()
        {
            sut = new AzureAssignUsertoGroupHandler(mockB2CGraphClient);
        }

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        [Fact]
        public async Task HandleReturnsTrue()
        {
            // Arrange
            var request = new AssignUsertoGroupModel
            {
                GroupObjectId = GroupObjectId,
                UserObjectId = UserObjectId
            };

            Mock.Arrange(() => mockB2CGraphClient.AssignUserToGroupAsync(request.GroupObjectId, request.UserObjectId)).Returns(Task.FromResult(true)).MustBeCalled();

            // Act
            var result = await sut.Handle(request, this.cancellationToken);

            // Asset
            result.Should().BeTrue();
            Mock.Assert(() => mockB2CGraphClient.AssignUserToGroupAsync(request.GroupObjectId, request.UserObjectId), Occurs.Once());
        }

        [Fact]
        public async Task HandleReturnsFalse()
        {
            // Arrange
            var request = new AssignUsertoGroupModel
            {
                GroupObjectId = GroupObjectId,
                UserObjectId = UserObjectId
            };

            Mock.Arrange(() => mockB2CGraphClient.AssignUserToGroupAsync(request.GroupObjectId, request.UserObjectId)).Returns(Task.FromResult(false)).MustBeCalled();

            // Act
            var result = await sut.Handle(request, this.cancellationToken);

            // Asset
            result.Should().BeFalse();
            Mock.Assert(() => mockB2CGraphClient.AssignUserToGroupAsync(request.GroupObjectId, request.UserObjectId), Occurs.Once());
        }
    }
}
