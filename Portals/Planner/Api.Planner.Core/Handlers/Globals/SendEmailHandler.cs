using Api.Planner.Core.Commands.Globals;
using Api.Planner.Core.Services.Globals;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Planner.Core.Handlers.Globals
{
    /// <summary>
    /// Manages to send email
    /// </summary>
    public class SendEmailHandler : IRequestHandler<EmailRequest, bool>
    {
        private readonly IConfiguration configuration;
        private readonly IGlobalsClient globalsClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailHandler"/> class.
        /// </summary>
        /// <param name="configuration">configuration object</param>
        /// <param name="globalsClient">globalsClient object</param>
        public SendEmailHandler(IConfiguration configuration, IGlobalsClient globalsClient)
        {
            this.configuration = configuration;
            this.globalsClient = globalsClient;
        }

        /// <summary>
        /// Method to handle the request
        /// </summary>
        /// <param name="request">The email request object</param>
        /// <param name="cancellationToken">Cancellation Token for the request</param>
        /// <returns>Returns bool value if email is send or no</returns>
        public async Task<bool> Handle(EmailRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var response = await globalsClient.SendEmailAsync(
            string.Format(configuration["ApiUri:Globals:SendEmail"]),
            JObject.Parse(JsonConvert.SerializeObject(request))).ConfigureAwait(false);

            return response != null && response.Value;
        }
    }
}