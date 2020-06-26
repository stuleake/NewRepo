using Api.Admin.Core.Commands.Blob;
using CT.Storage;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Admin.Core.Handlers.Blob
{
    /// <summary>
    /// Handler To Get Sas token
    /// </summary>
    public class GetSasTokenHandler : IRequestHandler<GetSasToken, string>
    {
        private readonly IStorageManager storageManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSasTokenHandler"/> class.
        /// </summary>
        /// <param name="storageManager">object of IStorageManager to perform blob operation</param>
        public GetSasTokenHandler(IStorageManager storageManager)
        {
            this.storageManager = storageManager;
        }

        /// <summary>
        /// Handle to Get Sas token
        /// </summary>
        /// <param name="request">object containing container name, file name and permissions to access the file</param>
        /// <param name="cancellationToken">cancellation Token to handle possible cancellation of a request</param>
        /// <returns>Returns the Sas Token as a string</returns>
        public async Task<string> Handle(GetSasToken request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Generate the sas token container/file specified with the permissions
            var token = await storageManager.GetSasTokenAsync(request.ContainerName, request.FileName, request.Permissions).ConfigureAwait(false);

            return token;
        }
    }
}