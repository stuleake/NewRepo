using Api.Planner.Core.Handlers.AddressSearch;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TQ.Core.Enums;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.Planner.Core.Handlers.AddressSearch
{
    /// <summary>
    /// Unit tests for the <see cref="GetAddressByPostcodeHandlerBase"/> class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetAddressByPostcodeHandlerBaseTests
    {
        private const string ValidPostcode = "NP10+0AA";

        /// <summary>
        /// GetRequestUri throws a <see cref="ArgumentNullException"/> when the country is null or white space
        /// </summary>
        [Fact]
        public void GetRequestUriThrowsArgumentNullExceptionWhenCountryIsNullOrWhiteSpace()
        {
            // Arrange
            IConfiguration configuration = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GetAddressByPostcodeHandlerBase.GetRequestUri(string.Empty, ValidPostcode, configuration, AddressTypes.Full));
        }

        /// <summary>
        /// GetRequestUri throws a <see cref="ArgumentNullException"/> when the postcode is null or white space
        /// </summary>
        [Fact]
        public void GetRequestUriThrowsArgumentNullExceptionWhenPostcodeIsNullOrWhiteSpace()
        {
            IConfiguration configuration = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GetAddressByPostcodeHandlerBase.GetRequestUri("england", string.Empty, configuration, AddressTypes.Full));
        }

        /// <summary>
        /// GetRequestUri throws a <see cref="ArgumentNullException"/> when the configuration is null
        /// </summary>
        [Fact]
        public void GetRequestUriThrowsArgumentNullExceptionWhenConfigurationIsNull()
        {
            IConfiguration configuration = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GetAddressByPostcodeHandlerBase.GetRequestUri("england", ValidPostcode, configuration, AddressTypes.Full));
        }

        /// <summary>
        /// GetRequestUri returns the correct uri
        /// </summary>
        /// <param name="country">the country to test</param>
        /// <param name="addressType">the address type to test</param>
        /// <param name="postcode">the postcode to test</param>
        /// <param name="expectedUri">the expected uri</param>
        [Theory]
        [InlineData("ENGLAND", AddressTypes.Full, ValidPostcode, "https://tq-geocoding-api-temp-dev.azurewebsites.net/addresses/full/by-postcode/english?postcode=NP10%2b0AA")]
        [InlineData("ENGLAND", AddressTypes.Simple, ValidPostcode, "https://tq-geocoding-api-temp-dev.azurewebsites.net/addresses/simple/by-postcode/english?postcode=NP10%2b0AA")]
        [InlineData("WALES", AddressTypes.Full, ValidPostcode, "https://tq-geocoding-api-temp-dev.azurewebsites.net/addresses/full/by-postcode/welsh?postcode=NP10%2b0AA")]
        [InlineData("WALES", AddressTypes.Simple, ValidPostcode, "https://tq-geocoding-api-temp-dev.azurewebsites.net/addresses/simple/by-postcode/welsh?postcode=NP10%2b0AA")]

        public void GetRequestUriSuccess(string country, AddressTypes addressType, string postcode, string expectedUri)
        {
            // Act
            IConfiguration configuration = UnitTestHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "ApiUri:Geocoding:BaseUrl", "tq-geocoding-api-temp-dev.azurewebsites.net" },
                { "ApiUri:Geocoding:GetEnglishFullAddressByPostcode", "addresses/full/by-postcode/english" },
                { "ApiUri:Geocoding:GetEnglishSimpleAddressByPostcode", "addresses/simple/by-postcode/english" },
                { "ApiUri:Geocoding:GetWelshFullAddressByPostcode", "addresses/full/by-postcode/welsh" },
                { "ApiUri:Geocoding:GetWelshSimpleAddressByPostcode", "addresses/simple/by-postcode/welsh" },
            });

            var actualUrl = GetAddressByPostcodeHandlerBase.GetRequestUri(country, postcode, configuration, addressType);

            // Assert
            actualUrl.AbsoluteUri.Should().Be(expectedUri);
        }
    }
}