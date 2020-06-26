using Api.Planner.Core.Commands.AddressSearch;
using Api.Planner.Core.Services.AddressSearch;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Enums;
using TQ.Core.Exceptions;

namespace Api.Planner.Core.Handlers.AddressSearch
{
    /// <summary>
    /// The GetFullAddressesByPostcodeHandler class
    /// </summary>
    public class GetFullAddressesByPostcodeHandler : GetAddressByPostcodeHandlerBase, IRequestHandler<GetFullAddressByPostcodeCommand, IEnumerable<FullAddressSearchModel>>
    {
        private readonly IGeocodingClient geocodingClient;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFullAddressesByPostcodeHandler"/> class.
        /// </summary>
        /// <param name="geocodingClient">the geocoding client to use</param>
        /// <param name="configuration">the client to use</param>
        public GetFullAddressesByPostcodeHandler(IGeocodingClient geocodingClient, IConfiguration configuration)
        {
            this.geocodingClient = geocodingClient;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FullAddressSearchModel>> Handle(GetFullAddressByPostcodeCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException($"{nameof(request)}");
            }

            ValidateRequest(request.Postcode, request.Country);
            Uri requestUri;

            try
            {
                requestUri = GetRequestUri(request.Country, request.Postcode, configuration, AddressTypes.Full);
            }
            catch (UriFormatException ex)
            {
                throw new TQException($"invalid configuration setting : {ex.Message}");
            }

            var response = await this.geocodingClient.GetFullAddressByPostCodeAsync(requestUri.AbsoluteUri, request.AuthToken).ConfigureAwait(false);

            if (response == null)
            {
                throw new ServiceException("Null response received from the geocoding client");
            }

            if (response.Code != (int)HttpStatusCode.OK)
            {
                throw new ServiceException($"Erroneous response received from the geocoding client ==> {response.Code} - {response.Message}");
            }

            return response.Value;
        }
    }
}