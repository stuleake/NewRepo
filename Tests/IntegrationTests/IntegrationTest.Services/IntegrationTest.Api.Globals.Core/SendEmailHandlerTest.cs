using System.Diagnostics.CodeAnalysis;

namespace IntegrationTest.Api.Globals.Core
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailHandlerTest"/> class.
        ///  constructor
        /// </summary>
        //// public SendEmailHandlerTest()
        //// {
        ////    // serviceProvider = IntegrationHelper.GiveServiceProvider();
        //// }

        /// <summary>
        /// Unit test for sending the Email
        /// </summary>
        /// <param name="emailid">email id</param>
        /// <param name="emailType">the template email type</param>
        /// <param name="name">name of the user</param>
        /// <returns>bool</returns>
        ////[Theory]
        ////[InlineData("cttqtestmail@yopmail.com", "Register", "Test")]
        ////public async Task Handle_SendEmail(string emailid, string emailType, string name)
        ////{
        ////    var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        ////    var appKey = _serviceProvider.GetRequiredService<IVaultManager>().GetSecret(configuration["SendGrid:AppKey"]);

        ////    var cmd = new GetEmailRequest() { EmailId = emailid, EmailType = emailType, Name = name };
        ////    EmailService emailService = new EmailService(appKey);

        ////    var handler = new SendEmailHandler(emailService, configuration, _serviceProvider.GetRequiredService<Log>());
        ////    var actual = await handler.Handle(cmd, new System.Threading.CancellationToken());
        ////    Assert.True(actual);
        ////}
    }
}