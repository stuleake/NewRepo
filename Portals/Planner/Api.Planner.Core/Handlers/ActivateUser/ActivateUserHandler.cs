using Api.Planner.Core.Commands.ActivateUser;
using Api.Planner.Core.Services.Globals;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Planner.Core.Handlers.ActivateUser
{
    /// <summary>
    /// Handler to activate user
    /// </summary>
    public class ActivateUserHandler : IRequestHandler<ActivateUserRequest, bool>
    {
        /// <summary>
        /// Global client object
        /// </summary>
        private readonly IGlobalsClient globalsClient;

        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivateUserHandler"/> class.
        /// </summary>
        /// <param name="globalsClient">globals client object</param>
        /// <param name="configuration">configuration data</param>
        public ActivateUserHandler(IGlobalsClient globalsClient, IConfiguration configuration)
        {
            this.globalsClient = globalsClient;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(ActivateUserRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await globalsClient.CreateUserAsync(
             string.Format(configuration["ApiUri:Globals:ActivateUser"]),
             JObject.Parse(JsonConvert.SerializeObject(request))).ConfigureAwait(false);

            return true;
        }
    }
}