using Api.Admin.Core.Commands.Blob;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Admin.Examples
{
    /// <summary>
    /// A class to get SasTokenRequest
    /// </summary>
    public class GetSasTokenRequest : IExamplesProvider<GetSasToken>
    {
        /// <summary>
        /// A method to get the example of SasToken object
        /// </summary>
        /// <returns>A sample object of GetSasToken</returns>
        public GetSasToken GetExamples()
        {
            return new GetSasToken
            {
                ContainerName = "Name of the container",
                FileName = "Name of the file",
                Permissions = "read | write"
            };
        }
    }
}