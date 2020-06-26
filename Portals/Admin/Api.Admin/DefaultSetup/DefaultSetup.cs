// ToDo - Pending Decision to move to poweshell script

//// using Api.Admin.Core.Commands.AzureUser;
//// using Api.Admin.Core.ViewModels;
//// using Api.Globals.Core.Helpers;
//// using CT.KeyVault;
//// using Microsoft.Extensions.Configuration;
//// using Newtonsoft.Json;
//// using System;
//// using System.Collections.Generic;
//// using System.Linq;
//// using System.Threading.Tasks;
//// using TQ.Core.Helper;

//// namespace Api.Admin.DefaultSetup
//// {
////     /// <summary>
////     /// Manages the Initial set up
////     /// </summary>
////     public class DefaultSetup
////     {
////         /// <summary>
////         /// Create the Default User in B2C
////         /// </summary>
////         /// <param name="client">object of B2CGraphClient</param>
////         /// <param name="mapper">object of AzureMapper</param>
////         /// <param name="configuration">configuration data</param>
////         /// <param name="initialisation">initialisation</param>
////         /// <param name="email">Emailid of default user</param>
////         /// <param name="password">password of default user</param>
////         /// <returns>""</returns>
////         public static async Task CreateDefaultUserAsync(B2CGraphClient client, AzureMapper mapper, IConfiguration configuration, bool initialisation, string email, string password)
////         {
////             if (client == null)
////             {
////                 throw new ArgumentNullException(nameof(client));
////             }
////             if (configuration == null)
////             {
////                 throw new ArgumentNullException(nameof(configuration));
////             }
////             if (initialisation)
////             {
////                 var keyvaultObject = VaultManagerProvider.CreateKeyVaultManager(KeyVaultTypes.MSI, configuration["KeyVault:BaseUrl"]);
////                 var existingUser = await client.GetUserByEmailAsync(email).ConfigureAwait(false);
////                 var existingUserObj = JsonConvert.DeserializeObject<AzureUserDataViewModel>(existingUser);

////                 if (existingUserObj.Value.Any())
////                 {
////                     return;
////                 }
////                 try
////                 {
////                     AzureUserModel model = new AzureUserModel
////                     {
////                         Title = "Mr",
////                         FirstName = "Super",
////                         LastName = "Admin",
////                         Email = email
////                     };

////                     // add in azure b2c
////                     var azureUserModel = MapAzureCreate(model, "P1@nningP0rt@1");
////                     string azureUser = JsonConvert.SerializeObject(azureUserModel);

////                     // add extension field
////                     azureUser = AddExtProperty(azureUser, keyvaultObject.GetSecret(configuration["ExtensionFieldTitle"]), model.Title);
////                     azureUser = AddExtProperty(azureUser, keyvaultObject.GetSecret(configuration["ExtensionFieldRole"]), "SuperAdmin");
////                     await client.CreateUserAsync(azureUser).ConfigureAwait(false);

////                     var newUser = await client.GetUserByEmailAsync(email).ConfigureAwait(false);
////                     var newUserObj = JsonConvert.DeserializeObject<AzureUserDataViewModel>(newUser);

////                     if (newUserObj.Value.Any())
////                     {
////                         await client.AssignUserToGroupAsync(keyvaultObject.GetSecret(configuration["AdminGroupId"]), newUserObj.Value.First().ObjectId);
////                         await client.AssignUserToGroupAsync(keyvaultObject.GetSecret(configuration["PlanningPortalGroupId"]), newUserObj.Value.First().ObjectId);
////                         await client.AssignUserToGroupAsync(keyvaultObject.GetSecret(configuration["LPAGroupId"]), newUserObj.Value.First().ObjectId);
////                     }
////                 }
////                 catch (Exception ex)
////                 {
////                     throw new Exception(ex.Message);
////                 }
////             }
////         }

////         /// <summary>
////         /// To add extension properties
////         /// </summary>
////         /// <param name="azureUser">string of azure user details</param>
////         /// <param name="fieldname">fieldName of the extension </param>
////         /// <param name="value">Value of the extension</param>
////         /// <returns>string of azure user</returns>
////         public static string AddExtProperty(string azureUser, string fieldname, string value)
////         {
////             if (string.IsNullOrEmpty(azureUser))
////             {
////                 throw new ArgumentNullException(nameof(azureUser));
////             }
////             int ind = azureUser.LastIndexOf("}", StringComparison.OrdinalIgnoreCase);
////             azureUser = azureUser.Remove(ind);
////             azureUser += string.Format(",\"{0}\":\"{1}\"", fieldname, value) + "}";
////             return azureUser;
////         }

////         /// <summary>
////         /// Manages to Map AzureUserModel to AzureUser
////         /// </summary>
////         /// <param name="createModel">object of AzureUser Model</param>
////         /// <param name="password">password</param>
////         /// <returns>object of AzureUser</returns>
////         public static AzureUser MapAzureCreate(AzureUserModel createModel, string password)
////         {
////             if (createModel == null)
////             {
////                 throw new ArgumentNullException(nameof(createModel));
////             }
////             AzureUser model = new AzureUser
////             {
////                 AccountEnabled = true
////             };
////             List<Api.Globals.Core.Helpers.SignInNames> names = new List<Api.Globals.Core.Helpers.SignInNames>();
////             Api.Globals.Core.Helpers.SignInNames name = new Api.Globals.Core.Helpers.SignInNames
////             {
////                 Type = "emailAddress",
////                 Value = createModel.Email
////             };
////             names.Add(name);
////             model.SignInNames = names;
////             model.CreationType = "LocalAccount";
////             string displayName = string.Format("{0} {1} ", createModel.FirstName, createModel.LastName);
////             model.DisplayName = string.IsNullOrEmpty(displayName) ? createModel.Email : displayName;
////             model.PasswordProfile = new PasswordProfile
////             {
////                 Password = password,
////                 ForceChangePasswordNextLogin = false
////             };

////             model.PasswordPolicies = "DisablePasswordExpiration";
////             return model;
////         }
////     }
//// }