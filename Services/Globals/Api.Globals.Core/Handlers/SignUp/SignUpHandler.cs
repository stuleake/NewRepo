using Api.Globals.Core.Commands.Email;
using Api.Globals.Core.Commands.SignUp;
using Api.Globals.Core.Handlers.Email;
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
using TQ.Core.Constants;
using TQ.Core.Enums;
using TQ.Core.Exceptions;
using TQ.Core.Helpers;

namespace Api.Globals.Core.Handlers.SignUp
{
    /// <summary>
    /// Handler to create unsigned user in Azure
    /// </summary>
    public class SignUpHandler : IRequestHandler<SignUpRequest, bool>
    {
        private readonly B2CGraphClient client;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Gets or Sets the azure mapper
        /// </summary>
        public AzureMapper Mapper { get; set; }

        private readonly ILogger<SendEmailHandler> logger;

        private readonly IVaultManager vaultManager;
        private readonly CeaserCipher ceaserCipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpHandler"/> class.
        /// </summary>
        /// <param name="client">object of B2CGraphClient containing the user details</param>
        /// <param name="mapper">object of AzureMapper</param>
        /// <param name="logger">Logger for the application</param>
        /// <param name="configuration">configuration data</param>
        /// <param name="vaultManager">object of VaultManager to access secret</param>
        /// <param name="globalsClient">Global client object</param>
        /// <param name="cipher">Ceaser cipher object</param>
        public SignUpHandler(B2CGraphClient client, AzureMapper mapper, ILogger<SendEmailHandler> logger, IConfiguration configuration, IVaultManager vaultManager, CeaserCipher cipher)
        {
            this.client = client;
            this.Mapper = mapper;
            this.configuration = configuration;
            this.vaultManager = vaultManager;
            this.logger = logger;
            this.ceaserCipher = cipher;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(SignUpRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var emailHandler = new SendEmailHandler(new EmailService(vaultManager.GetSecret(configuration["SendGrid:AppKey"])), configuration, logger, vaultManager, ceaserCipher, client);
            var existingUser = await client.GetUserByEmailAsync(request.Email).ConfigureAwait(false);
            var existingUserObj = JsonConvert.DeserializeObject<AzureUserDataViewModel>(existingUser);
            if (existingUserObj.Value.Any())
            {
                if (!existingUserObj.Value.First().AccountEnabled)
                {
                    throw new TQException("error_inactiveuserexists");
                }
                throw new TQException("error_userexists");
            }

            // add in azure b2c
            var azureUserModel = Mapper.MapAzureCreate(request);
            string azureUser = JsonConvert.SerializeObject(azureUserModel);

            // add extension field
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionFieldTitle"]), request.Title);
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionFieldRole"]), RoleTypes.StandardUser.ToString());
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionFieldAccountType"]), request.AccountType);
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionFieldProfessionType"]), request.ProfessionType);
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionFieldSecurityQuestion"]), request.SecurityQuestion);
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionFieldSecurityAnswer"]), request.SecurityAnswer);
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionFieldPlanningPortalEmail"]), request.PlanningPortalEmail.ToString());
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionFieldWeeklyPlanningEmailNewsLetter"]), request.WeeklyPlanningEmailNewsletter.ToString());
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionReceiveNewsOffers"]), request.ReceiveNewsOffers.ToString());
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionReceiveInfo"]), request.ReceiveInfo.ToString());
            azureUser = AddExtProperty(azureUser, vaultManager.GetSecret(configuration["ExtensionProduct"]), ProductConstants.PP2);

            await client.CreateUserAsync(azureUser).ConfigureAwait(false);

            var newUser = await client.GetUserByEmailAsync(request.Email).ConfigureAwait(false);
            var newUserObj = JsonConvert.DeserializeObject<AzureUserDataViewModel>(newUser);
            if (newUserObj.Value.Any())
            {
                await client.AssignUserToGroupAsync(
                    vaultManager.GetSecret(configuration["PlanningPortalGroupId"]),
                    newUserObj.Value.FirstOrDefault()?.ObjectId).ConfigureAwait(false);
            }

            GetEmailRequest emailRequest = new GetEmailRequest
            {
                EmailId = request.Email,
                EmailType = "Register",
                Name = request.FirstName
            };
            await emailHandler.Handle(emailRequest, CancellationToken.None).ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// To add extension properties
        /// </summary>
        /// <param name="azureUser">string of azure user details</param>
        /// <param name="fieldname">fieldName of the extension </param>
        /// <param name="value">Value of the extension</param>
        /// <returns>string of azure user</returns>
        public static string AddExtProperty(string azureUser, string fieldname, string value)
        {
            if (string.IsNullOrEmpty(azureUser))
            {
                throw new ArgumentNullException(nameof(azureUser));
            }
            int ind = azureUser.LastIndexOf("}", StringComparison.OrdinalIgnoreCase);
            azureUser = azureUser.Remove(ind);
            azureUser += string.Format(",\"{0}\":\"{1}\"", fieldname, value) + "}";
            return azureUser;
        }
    }
}