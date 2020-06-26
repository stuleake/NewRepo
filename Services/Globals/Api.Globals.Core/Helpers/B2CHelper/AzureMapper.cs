using Api.Globals.Core.Commands.SignUp;
using System;
using System.Collections.Generic;

namespace Api.Globals.Core.Helpers
{
    /// <summary>
    /// Mapper class to map azure user objects
    /// </summary>
    public class AzureMapper
    {
        /// <summary>
        /// Mapper method to map object from AzureUserModel to AzureUser
        /// </summary>
        /// <param name="createModel">object of AzureUserModel</param>
        /// <returns>Returns object of AzureUser</returns>
        public AzureUser MapAzureCreate(SignUpRequest createModel)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }
            AzureUser model = new AzureUser
            {
                AccountEnabled = false
            };
            List<SignInNames> names = new List<SignInNames>();
            SignInNames name = new SignInNames
            {
                Type = "emailAddress",
                Value = createModel.Email
            };
            names.Add(name);
            model.SignInNames = names;
            model.CreationType = "LocalAccount";
            string displayName = string.Format("{0} {1} ", createModel.FirstName, createModel.LastName);
            model.DisplayName = string.IsNullOrEmpty(displayName) ? createModel.Email : displayName;
            model.FirstName = createModel.FirstName;
            model.Surname = createModel.LastName;
            model.PasswordProfile = new PasswordProfile
            {
                Password = createModel.Password,
                ForceChangePasswordNextLogin = false
            };
            model.PasswordPolicies = "DisablePasswordExpiration";
            return model;
        }

        /// <summary>
        /// This method Generates the random password with all the required chars
        /// </summary>
        /// <returns>Returns the password as a string</returns>
        ////private static string GeneratePassword()
        ////{
        ////    const int length = 12;

        ////    bool nonAlphanumeric = true;
        ////    bool digit = true;
        ////    bool lowercase = true;
        ////    bool uppercase = true;
        ////    StringBuilder password = new StringBuilder();
        ////    Random random = new Random();

        ////    while (password.Length < length)
        ////    {
        ////        char c = (char)random.Next(32, 126);

        ////        password.Append(c);

        ////        if (char.IsDigit(c))
        ////        {
        ////            digit = false;
        ////        }
        ////        else if (char.IsLower(c))
        ////        {
        ////            lowercase = false;
        ////        }
        ////        else if (char.IsUpper(c))
        ////        {
        ////            uppercase = false;
        ////        }
        ////        else if (!char.IsLetterOrDigit(c))
        ////        {
        ////            nonAlphanumeric = false;
        ////        }
        ////    }

        ////    if (nonAlphanumeric)
        ////    {
        ////        const int nonAlphanumericLowerBound = 33;
        ////        const int nonAlphanumericUpperBound = 48;

        ////        password.Append((char)random.Next(nonAlphanumericLowerBound, nonAlphanumericUpperBound));
        ////    }

        ////    if (digit)
        ////    {
        ////        const int DigitLowerBound = 48;
        ////        const int DigitUpperBound = 58;
        ////        password.Append((char)random.Next(DigitLowerBound, DigitUpperBound));
        ////    }

        ////    if (lowercase)
        ////    {
        ////        const int LowercaseLowerBound = 97;
        ////        const int LowercaseUpperBound = 123;
        ////        password.Append((char)random.Next(LowercaseLowerBound, LowercaseUpperBound));
        ////    }

        ////    if (uppercase)
        ////    {
        ////        const int UppercaseLowerBound = 65;
        ////        const int UppercaseUpperBound = 91;
        ////        password.Append((char)random.Next(UppercaseLowerBound, UppercaseUpperBound));
        ////    }

        ////    return password.ToString();
        ////}
    }
}