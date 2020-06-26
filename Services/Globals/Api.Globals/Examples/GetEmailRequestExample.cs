using Api.Globals.Core.Commands.Email;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Globals.Examples
{
    /// <summary>
    /// Email request sample for swagger document
    /// </summary>
    public class GetEmailRequestExample : IExamplesProvider<GetEmailRequest>
    {
        /// <summary>
        /// Returns the example object for the email request
        /// </summary>
        /// <returns>Returns the GetEmailRequest object containing EmailId, EmailType and Name</returns>
        public GetEmailRequest GetExamples()
        {
            return new GetEmailRequest
            {
                EmailId = "Email Id",
                EmailType = "Email Type for mail",
                Name = "Name",
            };
        }
    }
}