using Api.Planner.Core.Commands.ApplicationList;
using Api.Planner.Core.Handlers.ApplicationList;
using Api.Planner.Core.Services.FormEngine;
using Api.Planner.Core.Services.PP2;
using Api.Planner.Core.ViewModels;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Constants;
using TQ.Core.Exceptions;
using TQ.Core.Models;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.Planner.Core.Handlers.ApplicationList
{
    /// <summary>
    /// Application List Handler Tests
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationListHandlerTests
    {
        private readonly IFormEngineClient mockFormHttpClient = Mock.Create<IFormEngineClient>(Behavior.Loose);
        private readonly IPP2HttpClient mockPP2HttpClient = Mock.Create<IPP2HttpClient>();
        private IConfiguration configuration;
        private readonly CancellationToken cancellationToken = CancellationToken.None;

        private const string RequestUrl = "/requesturl";
        private const string AuthorisationToken = "authorisation token";


        /// <summary>
        /// Test to throw an Argument Null Exception
        /// </summary>
        /// <returns>Test returns ArgumentNullException</returns>
        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        /// <summary>
        /// Test to throw a TQ Exception
        /// </summary>
        /// <returns>Test returns TQException with specific message</returns>
        [Fact]
        public async Task HandleThrowsTqExceptionWhenRequestTypeIsNotValid()
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

        /// <summary>
        /// Test to throw a Service Exception
        /// </summary>
        /// <returns>Test returns ServiceException with specific message</returns>
        [Fact]
        public async Task HandleThrowsServiceExceptionWhenFormHttpClientResponseIsNull()
        {
            // Arrange
            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:FormEngine:DraftApplicationListResponse", RequestUrl },
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

        /// <summary>
        /// Test to throw a Service Exception
        /// </summary>
        /// <returns>Test returns ServiceException with specific message</returns>
        [Fact]
        public async Task HandleThrowsServiceExceptionWhenPp2ClientResponseIsNull()
        {
            // Arrange
            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:PP2:SubmittedApplicationListResponse", RequestUrl },
            });

            ServiceResponse<ApplicationListModel> serviceResponse = null;

            var request = new ApplicationListRequestModel
            {
                Type = ApplicationStatusConstants.Submitted,
                AuthToken = AuthorisationToken,
                Country = "Wales"
            };

            var sut = this.CreateSut();

            Mock.Arrange(() => mockPP2HttpClient.GetSubmittedApplicationsAsync(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();

            // Act
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));

            // Assert
            ex.Message.Should().Be("Null response received from the http client");
            Mock.Assert(() => mockPP2HttpClient.GetSubmittedApplicationsAsync(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        private ApplicationListHandler CreateSut()
        {
            return new ApplicationListHandler(mockFormHttpClient, mockPP2HttpClient, configuration);
        }
    }
}