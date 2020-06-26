using MediatR;

namespace Api.Admin.Core.Commands.Blob
{
    /// <summary>
    /// Model to request a sas token
    /// </summary>
    public class GetSasToken : IRequest<string>
    {
        /// <summary>
        /// Gets or Sets Container Name for blob
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or Sets File Name for the container
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or Sets the Permission to access the file
        /// </summary>
        public string Permissions { get; set; }
    }
}