using Api.Planner.Core.Commands.AddressSearch;
using Api.Planner.Core.Handlers.AddressSearch;
using Api.Planner.Core.Services.AddressSearch;
using Api.Planner.Core.ViewModels;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Constants;
using TQ.Core.Exceptions;
using TQ.Core.Models;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.Planner.Core.Handlers.AddressSearch
{
    /// <summary>
    /// Unit tests for the <see cref="GetSimpleAddressesByPostcodeHandler"/> class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetSimpleAddressesByPostcodeHandlerTests
    {
        private readonly IGeocodingClient mockGeocodingClient = Mock.Create<IGeocodingClient>();
        private readonly CancellationToken cancellationToken = CancellationToken.None;
        private IConfiguration configuration;

        private const string ValidPostcode = "NP10 0AA";

        /// <summary>
        /// Handle throws a <see cref="ArgumentNullException"/> when the request is null
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        /// <summary>
        /// Handle throws a <see cref="ArgumentNullException"/> when the postcode is null or white space
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenPostCodeIsNullOrWhiteSpace()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = string.Empty };

            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(request, this.cancellationToken));
        }

        /// <summary>
        /// Handle throws a <see cref="ArgumentException"/> when the postcode is not valid
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsArgumentExceptionWhenPostCodeIsNotValid()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = "invalid postcode" };

            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, this.cancellationToken));
        }

        /// <summary>
        /// Handle throws a <see cref="ArgumentNullException"/> when the country is null or white space
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenCountryIsNullOrWhiteSpace()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = ValidPostcode, Country = string.Empty };

            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(request, this.cancellationToken));
        }

        /// <summary>
        /// Handle throws a <see cref="ArgumentException"/> when the country is not valid
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsArgumentExceptionWhenCountryIsNotValid()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = ValidPostcode, Country = "Invalid Country" };

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Geocoding:BaseUrl", "addresses/simple/by-postcode/english" },
            });

            var sut = this.CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, this.cancellationToken));
        }

        /// <summary>
        /// Handle throws a <see cref="ServiceException"/> when the response is null and the country is england
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsServiceExceptionWhenResponseIsNullEnglish()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = ValidPostcode, Country = CountryConstants.England };
            ServiceResponse<IEnumerable<SimpleAddressSearchModel>> serviceResponse = null;

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Geocoding:GetEnglishSimpleAddressByPostcode", "addresses/simple/by-postcode/english" },
            });

            Mock.Arrange(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();

            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));
            ex.Message.Should().Be("Null response received from the geocoding client");
            Mock.Assert(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        /// <summary>
        /// Handle throws a <see cref="ServiceException"/> when the response is null and the country is wales
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsServiceExceptionWhenResponseIsNullWelsh()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = ValidPostcode, Country = CountryConstants.Wales };
            ServiceResponse<IEnumerable<SimpleAddressSearchModel>> serviceResponse = null;

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Geocoding:GetWelshSimpleAddressByPostcode", "addresses/simple/by-postcode/english" },
            });

            Mock.Arrange(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();

            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));
            ex.Message.Should().Be("Null response received from the geocoding client");
            Mock.Assert(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        /// <summary>
        /// Handle throws a <see cref="ServiceException"/> when the response is not OK and the country is england
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsServiceExceptionWhenResponseIsNotOkEnglish()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = ValidPostcode, Country = CountryConstants.England };
            var serviceResponse = new ServiceResponse<IEnumerable<SimpleAddressSearchModel>>
            {
                Code = (int)HttpStatusCode.BadRequest,
                Message = "bad request"
            };

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Geocoding:GetEnglishSimpleAddressByPostcode", "addresses/simple/by-postcode/english" },
            });

            Mock.Arrange(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();

            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));
            ex.Message.Should().Be($"Erroneous response received from the geocoding client ==> {serviceResponse.Code} - {serviceResponse.Message}");
            Mock.Assert(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        /// <summary>
        /// Handle throws a <see cref="ServiceException"/> when the response is not OK and the country is wales
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsServiceExceptionWhenResponseIsNotOkWelsh()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = ValidPostcode, Country = CountryConstants.Wales };
            var serviceResponse = new ServiceResponse<IEnumerable<SimpleAddressSearchModel>>
            {
                Code = (int)HttpStatusCode.BadRequest,
                Message = "bad request"
            };

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Geocoding:GetWelshSimpleAddressByPostcode", "addresses/simple/by-postcode/welsh" },
            });

            Mock.Arrange(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();

            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ServiceException>(() => sut.Handle(request, this.cancellationToken));
            ex.Message.Should().Be($"Erroneous response received from the geocoding client ==> {serviceResponse.Code} - {serviceResponse.Message}");
            Mock.Assert(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        /// <summary>
        /// Handle throws a <see cref="TQException"/> when any of the ApiUri:Geocoding settings are not valid
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleThrowsTQExceptionWhenAppSeetingsAreNotValid()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = ValidPostcode, Country = CountryConstants.Wales };
            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Geocoding:GetWelshFullAddressByPostcode", "&&%%$$++" },
            });

            var sut = this.CreateSut();

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<TQException>(() => sut.Handle(request, this.cancellationToken));
            ex.Message.Should().Be("invalid configuration setting : Invalid URI: The hostname could not be parsed.");
        }

        /// <summary>
        /// Handle returns success for an english postcode
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleSuccessEnglish()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = ValidPostcode, Country = CountryConstants.England };
            var serviceResponse = new ServiceResponse<IEnumerable<SimpleAddressSearchModel>>
            {
                Code = (int)HttpStatusCode.OK,
                Value = new List<SimpleAddressSearchModel>()
            };

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Geocoding:GetEnglishSimpleAddressByPostcode", "addresses/simple/by-postcode/english" },
            });

            Mock.Arrange(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();

            var sut = this.CreateSut();

            // Act & Assert
            var actual = await sut.Handle(request, this.cancellationToken);
            actual.Should().BeEquivalentTo(serviceResponse.Value);
            Mock.Assert(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        /// <summary>
        /// Handle returns success for a welsh postcode
        /// </summary>
        /// <returns>a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task HandleSuccessWelsh()
        {
            // Arrange
            var request = new GetSimpleAddressByPostcodeCommand { Postcode = ValidPostcode, Country = CountryConstants.Wales };
            var serviceResponse = new ServiceResponse<IEnumerable<SimpleAddressSearchModel>>
            {
                Code = (int)HttpStatusCode.OK,
                Value = new List<SimpleAddressSearchModel>()
            };

            this.configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Geocoding:GetWelshSimpleAddressByPostcode", "addresses/simple/by-postcode/welsh" },
            });

            Mock.Arrange(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(serviceResponse)).MustBeCalled();

            var sut = this.CreateSut();

            // Act & Assert
            var actual = await sut.Handle(request, this.cancellationToken);
            actual.Should().BeEquivalentTo(serviceResponse.Value);
            Mock.Assert(() => mockGeocodingClient.GetSimpleAddressByPostCodeAsync(Arg.AnyString, Arg.AnyString), Occurs.Once());
        }

        private GetSimpleAddressesByPostcodeHandler CreateSut()
        {
            return new GetSimpleAddressesByPostcodeHandler(mockGeocodingClient, configuration);
        }
    }
}