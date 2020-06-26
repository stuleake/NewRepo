using Swashbuckle.AspNetCore.Filters;

namespace Api.Admin.Examples
{
    /// <summary>
    /// Azure User Group Example Class
    /// </summary>
    public class AzureUserGroupExample : IExamplesProvider<string>
    {
        /// <summary>
        /// The example of the uswer group
        /// </summary>
        /// <returns>A string representing the azure object id of the user</returns>
        public string GetExamples()
        {
            return "Object Id of user";
        }
    }
}