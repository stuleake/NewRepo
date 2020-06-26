using Api.Globals.Core.Commands.ActivateUser;
using Api.Globals.Core.Handlers.ActivateUser;
using IntegrationTest.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Core.Helpers;
using Xunit;

namespace IntegrationTest.Api.Globals.Core
{
    /// <summary>
    /// Test Handler for activating user
    /// </summary>
    public class ActivateUserHandlerTest
    {
        private B2CGraphClient Client { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivateUserHandlerTest"/> class.
        /// </summary>
        public ActivateUserHandlerTest()
        {
            var serviceProvider = IntegrationHelper.GiveServiceProvider();
            Client = serviceProvider.GetRequiredService<B2CGraphClient>();
        }

        /// <summary>
        /// Test Activate User
        /// </summary>
        /// <returns>A bool representing the result</returns>
        [Fact]
        public async Task Handle_ActivateUser()
        {
            var cmd = new ActivateUserRequest { EmailId = "tqsa@yopmail.com" };

            var handler = new ActivateUserHandler(Client);
            try
            {
                await handler.Handle(cmd, System.Threading.CancellationToken.None);
            }
            catch (TQException ex)
            {
                Assert.Equal("error_activeuserexist", ex.Message);
            }
        }
    }
}