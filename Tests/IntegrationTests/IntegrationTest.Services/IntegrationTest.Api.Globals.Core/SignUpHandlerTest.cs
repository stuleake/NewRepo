using Api.Globals.Core.Commands.SignUp;
using Api.Globals.Core.Handlers.Email;
using Api.Globals.Core.Handlers.SignUp;
using Api.Globals.Core.Helpers;
using CT.KeyVault;
using IntegrationTest.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Core.Helpers;
using Xunit;

namespace IntegrationTest.Api.Globals.Core
{
    /// <summary>
    /// Class to create Azure User unit test
    /// </summary>
    public class SignUpHandlerTest
    {
        private B2CGraphClient Client { get; set; }

        private AzureMapper Mapper { get; set; }

        private IConfiguration Configuration { get; set; }

        private ILogger<SendEmailHandler> Logger { get; set; }

        private IVaultManager Keyvault { get; set; }

        private CeaserCipher Cipher { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpHandlerTest"/> class.
        /// </summary>
        public SignUpHandlerTest()
        {
            var serviceProvider = IntegrationHelper.GiveServiceProvider();
            Client = serviceProvider.GetRequiredService<B2CGraphClient>();
            Mapper = serviceProvider.GetRequiredService<AzureMapper>();
            Configuration = serviceProvider.GetRequiredService<IConfiguration>();
            Logger = serviceProvider.GetRequiredService<ILogger<SendEmailHandler>>();
            Keyvault = serviceProvider.GetRequiredService<IVaultManager>();
            Cipher = serviceProvider.GetRequiredService<CeaserCipher>();
        }

        /// <summary>
        /// Integration test for signup
        /// </summary>
        /// <returns>bool</returns>
        [Fact]
        public async Task Handle_SignUp()
        {
            var handler = new SignUpHandler(Client, Mapper, Logger, Configuration, Keyvault, Cipher);
            SignUpRequest req = new SignUpRequest
            {
                Title = "Miss",
                FirstName = "Integration",
                LastName = "Test",
                AccountType = "test",
                Email = "tqsa@yopmail.com",
                ReceiveInfo = false,
                ReceiveNewsOffers = false,
                Password = "Qwerty@123",
                PlanningPortalEmail = true,
                ProfessionType = "landlord",
                SecurityQuestion = "What your mother's maiden name?",
                SecurityAnswer = "Asha",
                WeeklyPlanningEmailNewsletter = false
            };
            try
            {
                await handler.Handle(req, System.Threading.CancellationToken.None);
            }
            catch (TQException ex)
            {
                Assert.Equal("error_userexists", ex.Message);
            }
        }
    }
}