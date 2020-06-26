using Api.Planner.Core.Commands.Gatsby;
using Api.Planner.Core.Handlers.Gatsby;
using Api.Planner.Core.Services.Gatsby;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Exceptions;
using TQ.Core.Models;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.Planner.Core.Handlers.Gatsby
{
    [ExcludeFromCodeCoverage]
    public class GatsbyBuildHandlerTests
    {
        private readonly IGatsbyClient mockGatsbyClient = Mock.Create<IGatsbyClient>();
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
        public async Task HandleThrowsServiceExceptionWhenGatsbyClientResponseIsNull()
        {
            // Arrange
            var request = new GatsbyDefinition
            {
                Definition = new Definition
                {
                    Id = 1
                }
            };

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "GatsBy:AuthToken", "auth token" },
            });

            ServiceResponse<bool> response = null;

            Mock.Arrange(() => mockGatsbyClient.GatsbyBuildAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyBool)).Returns(Task.FromResult(response)).MustBeCalled();

            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));
            ex.Message.Should().Be("Null response received from the Gatsby engine");

            Mock.Assert(() => mockGatsbyClient.GatsbyBuildAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyBool));
        }

        [Theory]
        [InlineData((int)HttpStatusCode.OK, true)]
        [InlineData((int)HttpStatusCode.ServiceUnavailable, false)]
        public async Task HandleSuccess(int httpStatusCode, bool expected)
        {
            // Arrange
            var request = new GatsbyDefinition
            {
                Definition = new Definition
                {
                    Id = 1
                }
            };

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "GatsBy:AuthToken", "auth token" },
            });


            var response = new ServiceResponse<bool>
            {
                Code = httpStatusCode,
            };

            Mock.Arrange(() => mockGatsbyClient.GatsbyBuildAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyBool)).Returns(Task.FromResult(response)).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            var actual = await sut.Handle(request, this.cancellationToken);

            // Assert
            actual.Should().Be(expected);
            Mock.Assert(() => mockGatsbyClient.GatsbyBuildAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyBool));
        }

        private GatsbyBuildHandler CreateSut()
        {
            return new GatsbyBuildHandler(configuration, mockGatsbyClient, null);
        }
    }
}