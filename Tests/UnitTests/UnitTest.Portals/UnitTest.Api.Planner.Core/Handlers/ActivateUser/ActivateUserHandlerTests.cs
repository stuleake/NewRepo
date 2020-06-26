using Api.Planner.Core.Commands.ActivateUser;
using Api.Planner.Core.Handlers.ActivateUser;
using Api.Planner.Core.Services.Globals;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Models;
using Xunit;

namespace UnitTest.Api.Planner.Core.Handlers.ActivateUser
{
    [ExcludeFromCodeCoverage]
    public class ActivateUserHandlerTests
    {
        private readonly IGlobalsClient mockGlobalsClient = Mock.Create<IGlobalsClient>();
        private readonly IConfiguration mockConfiguration = Mock.Create<IConfiguration>();
        private readonly CancellationToken cancellationToken = CancellationToken.None;
        private readonly ActivateUserHandler sut;

        public ActivateUserHandlerTests()
        {
            this.sut = new ActivateUserHandler(mockGlobalsClient, mockConfiguration);
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
            var request = new ActivateUserRequest
            {
                EmailId = "testemailId"
            };

            var response = new ServiceResponse<bool>
            {
                Value = true
            };

            Mock.Arrange(() => mockGlobalsClient.CreateUserAsync(Arg.AnyString, Arg.IsAny<JObject>())).Returns(Task.FromResult(response)).MustBeCalled();

            // Act
            var actual = await sut.Handle(request, this.cancellationToken);

            // Assert
            actual.Should().Be(true);
            Mock.Assert(() => mockGlobalsClient.CreateUserAsync(Arg.AnyString, Arg.IsAny<JObject>()), Occurs.Once());
        }
    }
}