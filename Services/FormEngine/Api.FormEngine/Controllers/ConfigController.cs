using Api.FormEngine.Core.Commands.Config;
using Api.FormEngine.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Controllers;

namespace Api.FormEngine.Controllers
{
    /// <summary>
    /// Config Api to handle configuration related api calls
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : BaseApiController
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigController"/> class.
        /// </summary>
        /// <param name="configuration">object of microsoft configuration being passed using dependency injection</param>
        /// <param name="mediator">object of mediateR being passed using dependency injection</param>
        public ConfigController(IConfiguration configuration, IMediator mediator)
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }

        /// <summary>
        /// Gives the Keyvault entries related to the AzureAdB2C and appinsights intrumentation key
        /// </summary>
        /// <returns>Config object <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <response code="200">If successfully retuned the config settings</response>
        /// <response code="500">If any unhandled exception occurs</response>
        [HttpGet]
        [Produces(typeof(Config))]
        public async Task<ActionResult<Config>> GetConfigurationAsync()
        {
            var appInsights = configuration["AppInsightsInstrumentationKey"];

            var keys = new List<string>();
            configuration.Bind("AzureAdB2C", keys);
            keys.Add(appInsights);

            GetConfig cmd = new GetConfig { Keys = keys };

            return await mediator.Send(cmd);
        }
    }
}