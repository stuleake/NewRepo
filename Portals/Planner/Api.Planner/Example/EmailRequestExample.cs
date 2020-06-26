using Api.Planner.Core.Commands.Globals;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Planner.Example
{
    /// <summary>
    /// Email Request Example
    /// </summary>
    public class EmailRequestExample : IExamplesProvider<EmailRequest>
    {
        /// <summary>
        /// The example of the EmailRequest
        /// </summary>
        /// <returns>A EmailRequest object</returns>
        public EmailRequest GetExamples()
        {
            return new EmailRequest
            {
                EmailId = "Test@yopmail.com",
                EmailType = "Registers",
                Name = "Test",
            };
        }
    }
}