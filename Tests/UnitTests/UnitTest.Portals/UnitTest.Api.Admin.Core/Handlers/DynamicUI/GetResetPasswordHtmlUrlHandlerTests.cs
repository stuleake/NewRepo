using Api.Admin.Core.Commands.DynamicUI;
using Api.Admin.Core.Handlers.DynamicUI;
using Api.Admin.Core.ViewModels;
using CT.KeyVault;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.DynamicUI
{
    [ExcludeFromCodeCoverage]
    public class GetResetPasswordHtmlUrlHandlerTests
    {
        private readonly IVaultManager mockVaultManager = Mock.Create<IVaultManager>();
        private readonly IConfiguration mockConfiguration = Mock.Create<IConfiguration>();
        private readonly IDynamicHtmlPage mockDynamicHtml = Mock.Create<IDynamicHtmlPage>();
        private readonly System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
        private readonly GetResetPasswordHtmlUrlHandler sut;

        public GetResetPasswordHtmlUrlHandlerTests()
        {
            this.sut = new GetResetPasswordHtmlUrlHandler(mockVaultManager, mockConfiguration, mockDynamicHtml);
        }

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        [Theory]
        [InlineData("")]
        [InlineData("ENGLAND")]
        public async Task HandleSuccess(string country)
        {
            // Arrange
            var request = new GetResetPasswordHtmlUrl { Country = country };
            var countryUrl = "\\countryUrl";
            var expeced = "\\expectedUrl";

            Mock.Arrange(() => mockVaultManager.GetSecretAsync(Arg.AnyString)).Returns(Task.FromResult(countryUrl)).MustBeCalled();
            Mock.Arrange(() => mockDynamicHtml.GetDymanicHtmlPageAsync(countryUrl)).Returns(Task.FromResult(expeced)).MustBeCalled();

            // Act
            var actual = await sut.Handle(request, this.cancellationToken);

            // Assert
            actual.Should().Be(expeced);
            Mock.Assert(() => mockVaultManager.GetSecretAsync(Arg.AnyString), Occurs.Once());
            Mock.Assert(() => mockDynamicHtml.GetDymanicHtmlPageAsync(countryUrl), Occurs.Once());
        }
    }
}