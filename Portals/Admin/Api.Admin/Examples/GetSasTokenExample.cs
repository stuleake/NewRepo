using Api.Admin.Core.Commands.Blob;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Admin.Examples
{
    /// <summary>
    /// A class to get Sas Token Example
    /// </summary>
    public class GetSasTokenExample : IExamplesProvider<GetSasToken>
    {
        /// <summary>
        /// A method to get the example of sasToken object
        /// </summary>
        /// <returns>Returns the sample object of GetSasToken</returns>
        public GetSasToken GetExamples()
        {
            return new GetSasToken
            {
                ContainerName = "Documents",
                FileName = "TestFile.txt",
                Permissions = "read | write",
            };
        }
    }
}