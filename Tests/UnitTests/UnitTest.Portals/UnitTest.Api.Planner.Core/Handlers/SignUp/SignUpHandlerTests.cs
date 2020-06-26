using Api.Planner.Core.Commands.SignUp;
using Api.Planner.Core.Handlers.SignUp;
using Api.Planner.Core.Services.Globals;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Models;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.Planner.Core.Handlers.SignUp
{
    [ExcludeFromCodeCoverage]
    public class SignUpHandlerTests
    {
        private readonly IGlobalsClient mockGlobalsClient = Mock.Create<IGlobalsClient>();
        private readonly System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
        private IConfiguration configuration;

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        [Fact]
        public async Task HandleSuccess()
        {
            // Arrange
            var request = new SignUpRequest
            {
                FirstName = "test",
                LastName = "user"
            };

            var response = new ServiceResponse<bool>
            {
                Value = true
            };

            Mock.Arrange(() => mockGlobalsClient.CreateUserAsync(Arg.AnyString, Arg.IsAny<JObject>())).Returns(Task.FromResult(response)).MustBeCalled();

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Globals:SignUp", "\\requestUrl" },
            });

            var sut = this.CreateSut();

            // Act
            var actual = await sut.Handle(request, this.cancellationToken);

            // Assert
            actual.Should().BeTrue();
            Mock.Assert(() => mockGlobalsClient.CreateUserAsync(Arg.AnyString, Arg.IsAny<JObject>()));
        }

        private SignUpHandler CreateSut()
        {
            return new SignUpHandler(mockGlobalsClient, configuration);
        }
    }
}