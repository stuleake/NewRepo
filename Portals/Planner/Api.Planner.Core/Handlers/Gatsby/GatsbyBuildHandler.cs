using Api.Planner.Core.Commands.Gatsby;
using Api.Planner.Core.Services.Gatsby;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;

namespace Api.Planner.Core.Handlers.Gatsby
{
    /// <summary>
    /// Handler for  Gatsby Builder call
    /// </summary>
    public class GatsbyBuildHandler : IRequestHandler<GatsbyDefinition, bool>
    {
        private readonly IConfiguration configuration;
        private readonly IGatsbyClient gatsbyClient;
        private readonly ILogger<GatsbyBuildHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GatsbyBuildHandler"/> class.
        /// </summary>
        /// <param name="configuration">configuration object</param>
        /// <param name="gatsbyClient">GatsbyClient object</param>
        /// <param name="log">Logger for the application</param>
        public GatsbyBuildHandler(IConfiguration configuration, IGatsbyClient gatsbyClient, ILogger<GatsbyBuildHandler> log)
        {
            this.configuration = configuration;
            this.gatsbyClient = gatsbyClient;
            this.logger = log;
        }

        /// <summary>
        /// Method to handle the request
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="cancellationToken">Cancellation Token for the request</param>
        /// <returns>Returns bool value if builder started or no</returns>
        public async Task<bool> Handle(GatsbyDefinition request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            bool isBuildStarted = false;
            var response = await gatsbyClient.GatsbyBuildAsync(
                  string.Empty,
                  JObject.Parse(JsonConvert.SerializeObject(request)),
                  string.Format(configuration["GatsBy:AuthToken"]),
                  true).ConfigureAwait(false);

            if (response == null)
            {
                throw new ServiceException("Null response received from the Gatsby engine");
            }

            if (response.Code == (int)HttpStatusCode.OK)
            {
                isBuildStarted = true;
                logger?.LogInformation($"Gastby Build is triggered successfully, id: {request.Definition.Id}");
            }
            else
            {
                logger?.LogInformation($" Gastby Build  trigger Failed, id: {request.Definition.Id}");
            }

            return isBuildStarted;
        }
    }
}