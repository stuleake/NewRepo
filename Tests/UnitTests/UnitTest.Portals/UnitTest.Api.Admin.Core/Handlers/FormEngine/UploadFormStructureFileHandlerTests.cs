using Api.Admin.Core.Commands.FormEngine;
using Api.Admin.Core.Handlers.FormEngine;
using Api.Admin.Core.Services.FormEngine;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Exceptions;
using TQ.Core.Models;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.FormEngine
{
    [ExcludeFromCodeCoverage]
    public class UploadFormStructureFileHandlerTests
    {
        private readonly UploadFormStructureFileHandler sut;
        private readonly System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
        private readonly IFormEngineClient mockFormEngineClient = Mock.Create<IFormEngineClient>();
        private readonly IConfiguration mockConfiguration = Mock.Create<IConfiguration>();

        public UploadFormStructureFileHandlerTests()
        {
            this.sut = new UploadFormStructureFileHandler(mockConfiguration, mockFormEngineClient);
        }

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        [Fact]
        public async Task HandleThrowsServiceExceptionWhenClientResponseIsNull()
        {
            // Arrange
            ServiceResponse<string> response = null;
            Mock.Arrange(() => mockFormEngineClient
                                .UploadFormStructureFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(response)).MustBeCalled();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(GetRequest(), this.cancellationToken));
            ex.Message.Should().Be("Null response received from the form engine");
            Mock.Assert(() => mockFormEngineClient.UploadFormStructureFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        [Fact]
        public async Task HandleThrowsServiceExceptionWhenResponseNotOk()
        {
            // Arrange
            var response = UnitTestHelper<string>.GetServiceResponseInternalServerError();

            Mock.Arrange(() => mockFormEngineClient
                                .UploadFormStructureFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(response)).MustBeCalled();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(GetRequest(), this.cancellationToken));
            ex.Message.Should().Be($"Erroneous response received from the forms engine ==> {response.Code} - {response.Message}");
            Mock.Assert(() => mockFormEngineClient.UploadFormStructureFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        [Fact]
        public async Task HandleSuccess()
        {
            // Arrange
            var response = UnitTestHelper<string>.GetServiceResponseCreated("created");

            Mock.Arrange(() => mockFormEngineClient
                                .UploadFormStructureFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(response)).MustBeCalled();

            // Act & Assert
            var actual = await sut.Handle(GetRequest(), this.cancellationToken);
            actual.Should().Be(response.Value);
            Mock.Assert(() => mockFormEngineClient.UploadFormStructureFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        private static UploadFormStructureFile GetRequest()
        {
            return new UploadFormStructureFile
            {
                AuthToken = "AuthToken",
                Product = "Product"
            };
        }
    }
}
