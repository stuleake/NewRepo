using Api.Globals.Core.Commands.Email;
using Api.Globals.Core.Handlers.Email;
using Api.Globals.Core.Helpers;
using CT.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TQ.Core.Helpers;
using UnitTest.Helpers;
using UnitTest.Helpers.FakeClients;
using Xunit;

namespace UnitTest.Api.Globals.Core
{
    /// <summary>
    /// Unit test fot hte Send Email.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SendEmailHandlerTest
    {
        /// <summary>
        /// Mnager the service provider.
        /// </summary>
        private readonly ServiceProvider serviceProvider;

        private readonly IConfiguration config;
        private readonly ILogger<SendEmailHandler> logger;
        private readonly CeaserCipher cipher;
        private readonly B2CGraphClient b2cClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailHandlerTest"/> class.
        ///  constructor
        /// </summary>
        public SendEmailHandlerTest()
        {
            serviceProvider = UnitTestHelper.GiveServiceProvider();
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            logger = serviceProvider.GetRequiredService<ILogger<SendEmailHandler>>();
            cipher = serviceProvider.GetRequiredService<CeaserCipher>();
            b2cClient = serviceProvider.GetRequiredService<B2CGraphClient>();
        }

        /// <summary>
        /// Unit test for sending the Email
        /// </summary>
        /// <param name="emailid">email id</param>
        /// <param name="emailType">the template email type</param>
        /// <param name="name">name of the user</param>
        /// <returns>bool</returns>
        [Theory]
        [InlineData("cttqtestmail@yopmail.com", "Register", "Test")]
        public async Task Handle_SendEmail(string emailid, string emailType, string name)
        {
            // Arrange
            var vaultMgr = serviceProvider.GetRequiredService<IVaultManager>();
            var appKey = vaultMgr.GetSecret("testkey1");

            var cmd = new GetEmailRequest { EmailId = emailid, EmailType = emailType, Name = name };
            EmailService emailService = new EmailService(appKey)
            {
                SendgridClient = new FakeSendgridClient().SendgridClient
            };

            var handler = new SendEmailHandler(emailService, config, logger, vaultMgr, cipher, b2cClient);

            // Act
            var actual = await handler.Handle(cmd, System.Threading.CancellationToken.None);

            // Assert
            Assert.True(actual);
        }
    }
}