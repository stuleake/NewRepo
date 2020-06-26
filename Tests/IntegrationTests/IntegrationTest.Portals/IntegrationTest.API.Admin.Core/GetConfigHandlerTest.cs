using Api.Admin.Core.Commands.Config;
using Api.Admin.Core.Handlers.Config;
using CT.KeyVault;
using IntegrationTest.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.Api.Admin.Core
{
    /// <summary>
    /// class to get congif unit test
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetConfigHandlerTest
    {
        private readonly ServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetConfigHandlerTest"/> class.
        /// </summary>
        public GetConfigHandlerTest()
        {
            serviceProvider = IntegrationHelper.GiveServiceProvider();
        }

        /// <summary>
        /// method to test the get config handler
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expected">expected value</param>
        /// <returns>true if success</returns>
        [Theory]
        [InlineData("AP-AppInsights-Key", "1")]
        public async Task Handle_GetConfig(string key, string expected)
        {
            var cmd = new GetConfig { Keys = new List<string> { key } };

            var handler = new GetConfigHandler(serviceProvider.GetRequiredService<IVaultManager>());

            var actual = await handler.Handle(cmd, System.Threading.CancellationToken.None);
            Assert.Equal(expected, Convert.ToString(actual.Secrets.Count));
        }
    }
}