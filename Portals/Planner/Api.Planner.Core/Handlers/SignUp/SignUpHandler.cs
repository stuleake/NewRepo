using Api.Planner.Core.Commands.SignUp;
using Api.Planner.Core.Services.Globals;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Planner.Core.Handlers.SignUp
{
    /// <summary>
    /// Handler to Create inactive user
    /// </summary>
    public class SignUpHandler : IRequestHandler<SignUpRequest, bool>
    {
        private readonly IGlobalsClient globalsClient;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpHandler"/> class.
        /// </summary>
        /// <param name="configuration">configuration data</param>
        /// <param name="globalsClient">Global client object</param>
        public SignUpHandler(IGlobalsClient globalsClient, IConfiguration configuration)
        {
            this.globalsClient = globalsClient;
            this.configuration = configuration;
        }

        /// <summary>
        /// Handle the user sign up
        /// </summary>
        /// <param name="request">The user sign up request</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>Returns a bool representing the response</returns>
        public async Task<bool> Handle(SignUpRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException($"{nameof(request)}");
            }

            await globalsClient.CreateUserAsync(
             string.Format(configuration["ApiUri:Globals:SignUp"]),
             JObject.Parse(JsonConvert.SerializeObject(request))).ConfigureAwait(false);

            return true;
        }
    }
}