using Api.Admin.Core.Commands.FormEngine;
using Api.Admin.Core.Handlers.FormEngine;
using Api.Admin.Core.Services.FormEngine;
using Api.Admin.Core.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Exceptions;
using TQ.Core.Models;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.FormEngine
{
    [ExcludeFromCodeCoverage]
    public class UploadTaxonomyHandlerTests
    {
        private readonly UploadTaxonomyHanlder sut;
        private readonly System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
        private readonly IFormEngineClient mockFormEngineClient = Mock.Create<IFormEngineClient>();
        private readonly IConfiguration mockConfiguration = Mock.Create<IConfiguration>();

        public UploadTaxonomyHandlerTests()
        {
            sut =  new UploadTaxonomyHanlder(mockConfiguration, mockFormEngineClient);
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
            ServiceResponse<TaxonomyUploadResponse> response = null;
            Mock.Arrange(() => mockFormEngineClient
                                .UploadTaxonomyFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(response)).MustBeCalled();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(GetRequest(), this.cancellationToken));
            ex.Message.Should().Be("Null response received from the forms engine");
            Mock.Assert(() => mockFormEngineClient.UploadTaxonomyFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        [Fact]
        public async Task HandleThrowsServiceExceptionWhenResponseNotOk()
        {
            // Arrange
            var response = new ServiceResponse<TaxonomyUploadResponse>
            {
                Code = 500,
                Message = "Error message"
            };

            Mock.Arrange(() => mockFormEngineClient
                                .UploadTaxonomyFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(response)).MustBeCalled();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(GetRequest(), this.cancellationToken));
            ex.Message.Should().Be($"Erroneous response received from the forms engine ==> {response.Code} - {response.Message}");
            Mock.Assert(() => mockFormEngineClient.UploadTaxonomyFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        [Fact]
        public async Task HandleSuccess()
        {
            // Arrange
            var response = new ServiceResponse<TaxonomyUploadResponse>
            {
                Code = (int)HttpStatusCode.Created,
                Value = new TaxonomyUploadResponse
                {
                    Response = "TaxonomyUploadResponse"
                }
            };

            Mock.Arrange(() => mockFormEngineClient
                                .UploadTaxonomyFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(response)).MustBeCalled();

            // Act & Assert
            var actual = await sut.Handle(GetRequest(), this.cancellationToken);
            actual.Should().Be(response.Value);
            Mock.Assert(() => mockFormEngineClient.UploadTaxonomyFileAsync(Arg.AnyString, Arg.IsAny<IFormFile>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        private UploadTaxonomy GetRequest()
        {
            return new UploadTaxonomy
            {
                AuthToken = "AuthToken",
                FileContent = null,
                Product = "Product"
            };
        }
    }
}