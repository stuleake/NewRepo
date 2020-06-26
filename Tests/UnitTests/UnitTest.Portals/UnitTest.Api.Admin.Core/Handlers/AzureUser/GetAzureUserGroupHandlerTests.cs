using Api.Admin.Core.Commands.AzureUser;
using Api.Admin.Core.Handlers.AzureUser;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Helpers;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.AzureUser
{
    [ExcludeFromCodeCoverage]
    public class GetAzureUserGroupHandlerTests
    {
        private readonly IB2CGraphClient mockB2CGraphClient = Mock.Create<IB2CGraphClient>();
        private readonly CancellationToken cancellationToken = new CancellationToken();
        private readonly GetAzureUserGroupHandler sut;

        public GetAzureUserGroupHandlerTests()
        {
            this.sut = new GetAzureUserGroupHandler(mockB2CGraphClient);
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
            var request = new AzureUserGroupHtmlUrl
            {
                ObjectId = "objectId"
            };

            IEnumerable<string> expected = new List<string> { "group1", "group2", "group3" };

            Mock.Arrange(() => mockB2CGraphClient.GetUserGroupByObjectIdAsync(request.ObjectId)).Returns(Task.FromResult(expected)).MustBeCalled();

            // Act
            var actual = await sut.Handle(request, this.cancellationToken);

            // Assert
            Mock.Assert(() => mockB2CGraphClient.GetUserGroupByObjectIdAsync(request.ObjectId), Occurs.Once());
            actual.Should().Equal(expected);
        }
    }
}
