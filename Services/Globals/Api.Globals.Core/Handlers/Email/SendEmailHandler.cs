using Api.Globals.Core.Commands.Email;
using Api.Globals.Core.Enums;
using Api.Globals.Core.Helpers;
using Api.Globals.Core.ViewModels;
using CT.KeyVault;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Helpers;

namespace Api.Globals.Core.Handlers.Email
{
    /// <summary>
    /// Send email Handler class
    /// </summary>
    public class SendEmailHandler : IRequestHandler<GetEmailRequest, bool>
    {
        private readonly IConfiguration configuration;
        private readonly EmailService emailService;
        private readonly ILogger<SendEmailHandler> logger;
        private readonly IVaultManager vaultManager;
        private readonly CeaserCipher ceaserCipher;
        private readonly B2CGraphClient b2cClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailHandler"/> class
        /// </summary>
        /// <param name="service">object of emailService being passed using dependency injection</param>
        /// <param name="config">object of configuration being passed using dependency injection</param>
        /// <param name="log">object of log being passed using dependency injection</param>
        /// <param name="keyvault">object of key vault being passed using dependency injection</param>
        /// <param name="cipher">object of ceaser cipher being passed using dependency injection</param>
        /// <param name="client">object of b2c client being passed using dependency injection</param>
        public SendEmailHandler(EmailService service, IConfiguration config, ILogger<SendEmailHandler> log, IVaultManager keyvault, CeaserCipher cipher, B2CGraphClient client)
        {
            emailService = service;
            configuration = config;
            logger = log;
            vaultManager = keyvault;
            ceaserCipher = cipher;
            b2cClient = client;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(GetEmailRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                var existingUser = await b2cClient.GetUserByEmailAsync(request.EmailId).ConfigureAwait(false);
                var existingUserObj = JsonConvert.DeserializeObject<AzureUserDataViewModel>(existingUser);
                request.Name = existingUserObj.Value.First().DisplayName;
            }

            bool isEmailsent = false;
            var enumExists = Enum.TryParse<EmailType>(request.EmailType, out EmailType emailType);
            if (enumExists)
            {
                string templatetypeId = GetTemplate("en", emailType);

                // to get the template id for the email
                var templateid = vaultManager.GetSecret(string.Format(configuration["SendGrid:Template:TemplateId-" + templatetypeId]));
                var from = string.Format(configuration["SendGrid:Email-From"]);

                // email template request parameters
                var emailbody = CreateEmailBody(request, emailType);
                isEmailsent = await emailService.SendEmailAsync(request.EmailId, emailbody, from, templateid).ConfigureAwait(false);
                if (isEmailsent)
                {
                    logger.LogInformation($"Mail is sent to: {request.Name} - {request.EmailId} Subject : {emailbody.Subject}");
                }
            }

            return isEmailsent;
        }

        private static string GetTemplate(string language, EmailType emailType)
        {
            string templateType = string.Empty;
            switch (emailType)
            {
                case EmailType.Register:
                    templateType = "RegisterSetPassword";
                    break;
            }

            language ??= "en";
            templateType = templateType + "-" + language;
            return templateType;
        }

        private EmailBodyModel CreateEmailBody(GetEmailRequest emailrequest, EmailType emailType)
        {
            if (emailrequest == null)
            {
                throw new ArgumentNullException(nameof(emailrequest));
            }
            EmailBodyModel emailbody = null;
            switch (emailType)
            {
                case EmailType.Register:
                    var resetLink = vaultManager.GetSecret(string.Format(configuration["SendGrid:Email-ResetLink"]));
                    var encryptedEmail = ceaserCipher.EncryptEmail(emailrequest.EmailId);
                    var encryptedTime = ceaserCipher.EncryptEmail(DateTime.UtcNow.ToString("MM/dd/yyyy H:mm:ss"));
                    var link = $"{resetLink}{encryptedEmail}/{encryptedTime}";
                    emailbody = new EmailBodyModel
                    {
                        Name = emailrequest.Name,
                        Link = link,
                        Subject = configuration["SendGrid:Template:Subject-" + emailType]
                    };
                    break;
            }

            return emailbody;
        }
    }
}