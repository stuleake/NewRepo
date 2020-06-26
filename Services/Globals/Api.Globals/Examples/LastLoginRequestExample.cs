using Api.Globals.Core.Commands.LastLogin;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Globals.Examples
{
    /// <summary>
    /// Example LastLoginRequest class
    /// </summary>
    public class LastLoginRequestExample : IExamplesProvider<LastLoginRequest>
    {
        /// <summary>
        /// Returns the example object for the last login request
        /// </summary>
        /// <returns>Returns the LastLoginRequest object containing EmailId</returns>
        public LastLoginRequest GetExamples()
        {
            return new LastLoginRequest
            {
                EmailId = "Email Id"
            };
        }
    }
}