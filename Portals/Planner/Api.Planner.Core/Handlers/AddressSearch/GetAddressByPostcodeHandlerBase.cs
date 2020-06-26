using Microsoft.Extensions.Configuration;
using System;
using System.Web;
using TQ.Core.Constants;
using TQ.Core.Enums;
using TQ.Core.Extensions;

namespace Api.Planner.Core.Handlers.AddressSearch
{
    /// <summary>
    /// Base class for the get address by postcode handlers
    /// </summary>
    public class GetAddressByPostcodeHandlerBase
    {
        /// <summary>
        /// Validate the handler request
        /// </summary>
        /// <param name="postcode">the postcode to validate</param>
        /// <param name="country">the country to validate</param>
        public static void ValidateRequest(string postcode, string country)
        {
            if (string.IsNullOrWhiteSpace(postcode))
            {
                throw new ArgumentNullException($"{nameof(postcode)}");
            }

            if (!postcode.IsPostCode())
            {
                throw new ArgumentException($"{postcode} is not a valid postcode");
            }

            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentNullException($"{country}");
            }
        }

        /// <summary>
        /// Gets the request Uri
        /// </summary>
        /// <param name="country">the country to use</param>
        /// <param name="postcode">the postcode to use</param>
        /// <param name="configuration">the configuration to use</param>
        /// <param name="addressType">the address type to use</param>
        /// <returns>the request Uri</returns>
        public static Uri GetRequestUri(string country, string postcode, IConfiguration configuration, AddressTypes addressType)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentNullException($"{nameof(country)}");
            }

            if (string.IsNullOrWhiteSpace(postcode))
            {
                throw new ArgumentNullException($"{nameof(postcode)}");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException($"{nameof(configuration)}");
            }

            var uriBuilder = new UriBuilder
            {
                Host = $"{configuration["ApiUri:Geocoding:BaseUrl"]}",
                Scheme = "https"
            };

            if (country.ToLower() == CountryConstants.England.ToLower())
            {
                if (addressType == AddressTypes.Full)
                {
                    uriBuilder.Path = $"{configuration["ApiUri:Geocoding:GetEnglishFullAddressByPostcode"]}";
                }
                else if (addressType == AddressTypes.Simple)
                {
                    uriBuilder.Path = $"{configuration["ApiUri:Geocoding:GetEnglishSimpleAddressByPostcode"]}";
                }
            }
            else if (country.ToLower() == CountryConstants.Wales.ToLower())
            {
                if (addressType == AddressTypes.Full)
                {
                    uriBuilder.Path = $"{configuration["ApiUri:Geocoding:GetWelshFullAddressByPostcode"]}";
                }
                else if (addressType == AddressTypes.Simple)
                {
                    uriBuilder.Path = $"{configuration["ApiUri:Geocoding:GetWelshSimpleAddressByPostcode"]}";
                }
            }
            else
            {
                throw new ArgumentException($"{country} is not valid.");
            }

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["postcode"] = postcode;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }
}