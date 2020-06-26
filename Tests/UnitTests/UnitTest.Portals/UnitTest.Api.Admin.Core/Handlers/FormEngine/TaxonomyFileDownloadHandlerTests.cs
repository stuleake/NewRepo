using Api.Admin.Core.Commands.FormEngine;
using Api.Admin.Core.Handlers.FormEngine;
using Api.Admin.Core.Services.FormEngine;
using Api.Admin.Core.ViewModels;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
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
    public class TaxonomyFileDownloadHandlerTests
    {
        private readonly TaxonomyFileDownloadHandler sut;
        private readonly System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
        private readonly IFormEngineClient mockFormEngineClient = Mock.Create<IFormEngineClient>();
        private readonly IConfiguration mockConfiguration = Mock.Create<IConfiguration>();

        public TaxonomyFileDownloadHandlerTests()
        {
            this.sut = new TaxonomyFileDownloadHandler(mockConfiguration, mockFormEngineClient);
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
            ServiceResponse<DownloadCsv> serviceResponse = null;
            Mock.Arrange(() => mockFormEngineClient
                                .TaxonomyFileDownloadAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(serviceResponse)).MustBeCalled();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(GetRequest(), this.cancellationToken));
            ex.Message.Should().Be("Null response received from the form engine");
            Mock.Assert(() => mockFormEngineClient.TaxonomyFileDownloadAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        [Fact]
        public async Task HandleThrowsServiceExceptionWhenResponseNotOk()
        {
            // Arrange
            var response = UnitTestHelper<DownloadCsv>.GetServiceResponseInternalServerError();

            Mock.Arrange(() => mockFormEngineClient
                                .TaxonomyFileDownloadAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(response)).MustBeCalled();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(GetRequest(), this.cancellationToken));
            ex.Message.Should().Be($"Erroneous response received from the forms engine ==> {response.Code} - {response.Message}");
            Mock.Assert(() => mockFormEngineClient.TaxonomyFileDownloadAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        [Fact]
        public async Task HandleThrowsTQExceptionWhenBadResponse()
        {
            // Arrange
            var response = UnitTestHelper<DownloadCsv>.GetServiceResponseBadRequest();

            Mock.Arrange(() => mockFormEngineClient
                                .TaxonomyFileDownloadAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(response)).MustBeCalled();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<TQException>(() => sut.Handle(GetRequest(), this.cancellationToken));
            ex.Message.Should().Be(response.Message);
            Mock.Assert(() => mockFormEngineClient.TaxonomyFileDownloadAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        [Fact]
        public async Task HandleSuccess()
        {
            // Arrange
            var response = UnitTestHelper<DownloadCsv>.GetServiceResponseSuccess(new DownloadCsv { CsvString = "csv string" });
            
            Mock.Arrange(() => mockFormEngineClient
                                .TaxonomyFileDownloadAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString))
                                .Returns(Task.FromResult(response)).MustBeCalled();

            // Act & Assert
            var actual = await sut.Handle(GetRequest(), this.cancellationToken);
            actual.Should().Be(response.Value);
            Mock.Assert(() => mockFormEngineClient.TaxonomyFileDownloadAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        private static TaxonomyFileDownload GetRequest()
        {
            return new TaxonomyFileDownload
            {
                AuthToken = "AuthToken",
                Product = "Product",
                QsNo = "QsNo",
                QsVersion = "QsVersion"
            };
        }
    }
}
