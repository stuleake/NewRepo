using Api.Admin.Core.Commands.ApplicationList;
using Api.Admin.Core.Handlers.ApplicationList;
using Api.Admin.Core.Services.FormEngine;
using Api.Admin.Core.Services.PP2;
using Api.Admin.Core.ViewModels;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Constants;
using TQ.Core.Exceptions;
using TQ.Core.Models;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.ApplicationList
{
    [ExcludeFromCodeCoverage]
    public class ApplicationListHandlerTests
    {
        private readonly IFormEngineClient mockFormHttpClient = Mock.Create<IFormEngineClient>(Behavior.Loose);
        private readonly IPP2HttpClient mockPP2HttpClient = Mock.Create<IPP2HttpClient>();

        private IConfiguration configuration;
        private readonly System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();

        private const string RequestUrl = "/requesturl";
        private const string AuthorisationToken = "authorisation token";

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        [Fact]
        public async Task HandleThrowsTQExceptionWhenRequestTypeIsNotValid()
        {
            // Arrange
            var request = new ApplicationListRequestModel
            {
                Type = "not a valid type"
            };

            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<TQException>(() => sut.Handle(request, this.cancellationToken));
            ex.Message.Should().Be($"Invalid application type {request.Type} in request.");
        }

        [Fact]
        public async Task HandleThrowsServiceExceptionWhenFormHttpClientResponseIsNull()
        {
            // Arrange
            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                {"ApiUri:FormEngine:DraftApplicationListResponse", RequestUrl},
            });

            var request = new ApplicationListRequestModel
            {
                Type = ApplicationStatusConstants.Drafts,
                AuthToken = AuthorisationToken,
            };

            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));
            ex.Message.Should().Be("Null response received from the forms engine");
        }

        [Fact]
        public async Task HandleThrowsServiceExceptionWhenPP2ClientResponseIsNull()
        {
            // Arrange
            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                {"ApiUri:PP2:SubmittedApplicationListResponse", RequestUrl},
            });

            ServiceResponse<ApplicationListModel> serviceResponse = null;

            var request = new ApplicationListRequestModel
            {
                Type = ApplicationStatusConstants.Submitted,
                AuthToken = AuthorisationToken,
                Country = "Wales"
            };

            Mock.Arrange(() => mockPP2HttpClient.GetSubmittedApplicationsAsync(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();

            var sut = this.CreateSut();

            // Act
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));
            
            // Assert
            ex.Message.Should().Be("Null response received from the http client");
            Mock.Assert(() => mockPP2HttpClient.GetSubmittedApplicationsAsync(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        [Fact]
        public async Task HandleThrowsServiceExceptionWhenFormHttpClientResponseNotValid()
        {
            // Arrange
            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                {"ApiUri:FormEngine:DraftApplicationListResponse", RequestUrl},
            });

            ServiceResponse<ApplicationListModel> serviceResponse = new ServiceResponse<ApplicationListModel>
            {
               Code = 500,
               Message = "service response message"
            };

            var request = new ApplicationListRequestModel
            {
                Type = ApplicationStatusConstants.Drafts,
                AuthToken = AuthorisationToken,
            };

            Mock.Arrange(() => mockFormHttpClient.GetDraftApplicationListAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));
            
            // Assert
            ex.Message.Should().Be($"Erroneous response received from the forms engine ==> {serviceResponse.Code} - {serviceResponse.Message}");
            Mock.Assert(() => mockFormHttpClient.GetDraftApplicationListAsync(Arg.AnyString, Arg.IsAny<JObject>(), Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        [Fact]
        public async Task HandleThrowsServiceExceptionWhenPP2HttpClientResponseNotValid()
        {
            // Arrange
            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                {"ApiUri:PP2:SubmittedApplicationListResponse", RequestUrl},
            });

            ServiceResponse<ApplicationListModel> serviceResponse = new ServiceResponse<ApplicationListModel>
            {
                Code = 500,
                Message = "service response message"
            };

            var request = new ApplicationListRequestModel
            {
                Type = ApplicationStatusConstants.Submitted,
                AuthToken = AuthorisationToken,
            };

            Mock.Arrange(() => mockPP2HttpClient.GetSubmittedApplicationsAsync(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();
            var sut = this.CreateSut();

            // Act
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));
            
            // Assert
            ex.Message.Should().Be($"Erroneous response received from the forms engine ==> {serviceResponse.Code} - {serviceResponse.Message}");
            Mock.Assert(() => mockPP2HttpClient.GetSubmittedApplicationsAsync(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        private ApplicationListHandler CreateSut()
        {
            return new ApplicationListHandler(this.mockFormHttpClient, this.mockPP2HttpClient, this.configuration);
        }
    }
}
