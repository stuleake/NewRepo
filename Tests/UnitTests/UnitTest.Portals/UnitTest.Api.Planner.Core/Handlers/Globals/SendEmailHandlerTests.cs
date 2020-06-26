using Api.Planner.Core.Commands.Globals;
using Api.Planner.Core.Handlers.Globals;
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

namespace UnitTest.Api.Planner.Core.Handlers.Globals
{
    [ExcludeFromCodeCoverage]
    public class SendEmailHandlerTests
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task HandleSuccess(bool value)
        {
            // Arrange
            var request = new EmailRequest
            {
                EmailId = "email id",
                EmailType = "email type",
                Name = "email name"
            };

            var response = new ServiceResponse<bool>
            {
                Value = value
            };

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Globals:SendEmail", "\\requestUrl" },
            });

            var sut = this.CreateSut();

            Mock.Arrange(() => mockGlobalsClient.SendEmailAsync(Arg.AnyString, Arg.IsAny<JObject>())).Returns(Task.FromResult(response)).MustBeCalled();

            // Act
            var actual = await sut.Handle(request, this.cancellationToken);

            // Assert
            actual.Should().Be(value);
            Mock.Assert(() => mockGlobalsClient.SendEmailAsync(Arg.AnyString, Arg.IsAny<JObject>()));
        }

        private SendEmailHandler CreateSut()
        {
            return new SendEmailHandler(configuration, mockGlobalsClient);
        }
    }
}