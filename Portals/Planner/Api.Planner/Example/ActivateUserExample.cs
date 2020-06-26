using Api.Planner.Core.Commands.ActivateUser;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Planner.Example
{
    /// <summary>
    /// Example of Activate user request model
    /// </summary>
    public class ActivateUserExample : IExamplesProvider<ActivateUserRequest>
    {
        /// <summary>
        /// The example of the activating user
        /// </summary>
        /// <returns>A activation object</returns>
        public ActivateUserRequest GetExamples()
        {
            return new ActivateUserRequest { EmailId = "Email id of user" };
        }
    }
}