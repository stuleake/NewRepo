using Api.Planner.Core.Commands.SignUp;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Planner.Example
{
    /// <summary>
    /// SignUp Example Class
    /// </summary>
    public class SignUpExample : IExamplesProvider<SignUpRequest>
    {
        /// <summary>
        /// The example of the signup
        /// </summary>
        /// <returns>A signup object</returns>
        public SignUpRequest GetExamples()
        {
            return new SignUpRequest
            {
                Title = "Title of user",
                Email = "EmailId of user",
                FirstName = "Firstname of user",
                LastName = "Lastname of user",
                Password = "Password of user",
                AccountType = "Accoount type",
                PlanningPortalEmail = false,
                ProfessionType = "Profession Type",
                ReceiveInfo = false,
                ReceiveNewsOffers = false,
                SecurityQuestion = "Security Question",
                SecurityAnswer = "Security Answer",
                WeeklyPlanningEmailNewsletter = false
            };
        }
    }
}