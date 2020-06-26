using Api.Admin.Core.Commands.AzureUser;
using Api.Admin.Core.Handlers.AzureUser;
using IntegrationTest.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TQ.Core.Helpers;
using Xunit;

namespace IntegrationTest.API.Admin.Core
{
    /// <summary>
    /// Class to Get Azure User unit test
    /// </summary>
    public class GetAzureUserHandlerTest
    {
        private readonly ServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAzureUserHandlerTest"/> class.
        /// </summary>
        public GetAzureUserHandlerTest()
        {
            serviceProvider = IntegrationHelper.GiveServiceProvider();
        }

        /// <summary>
        /// method to get azure user unit test
        /// </summary>
        /// <param name="active">request true to get active users</param>
        /// <returns>true if success</returns>
        [Theory]
        [InlineData(true)]
        public async Task Handle_GetAzureUser(bool active)
        {
            var cmd = new AzureUserHtmlUrl { Active = active };

            var handler = new GetAzureUserHandler(serviceProvider.GetRequiredService<B2CGraphClient>());

            var actual = await handler.Handle(cmd, System.Threading.CancellationToken.None);
            Assert.NotNull(actual);
        }
    }
}