using Api.Globals.Core.Commands.LastLogin;
using Api.Globals.Core.Handlers.LastLogin;
using CT.KeyVault;
using IntegrationTest.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TQ.Core.Helpers;
using Xunit;

namespace IntegrationTest.Api.Globals.Core
{
    /// <summary>
    /// Test Handler for last login of  user
    /// </summary>
    public class LastLoginHandlerTest
    {
        private readonly ServiceProvider serviceProvider;

        private B2CGraphClient Client { get; set; }

        private IConfiguration Configuration { get; set; }

        private IVaultManager Keyvault { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LastLoginHandlerTest"/> class.
        /// </summary>
        public LastLoginHandlerTest()
        {
            serviceProvider = IntegrationHelper.GiveServiceProvider();
            Client = serviceProvider.GetRequiredService<B2CGraphClient>();
            Configuration = serviceProvider.GetRequiredService<IConfiguration>();
            Keyvault = serviceProvider.GetRequiredService<IVaultManager>();
        }

        /// <summary>
        /// Integration test for last login
        /// </summary>
        /// <returns>A bool representing the result</returns>
        [Fact]
        public async Task Handle_LastLogin()
        {
            var cmd = new LastLoginRequest { EmailId = "integrationtest@test.com" };
            var handler = new LastLoginHandler(Client, Keyvault, Configuration);
            var result = await handler.Handle(cmd, System.Threading.CancellationToken.None);
            Assert.True(result);
        }
    }
}