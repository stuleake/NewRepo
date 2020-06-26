using Api.Globals.Core.Commands.DocumentUpload;
using CT.Storage;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;

namespace Api.Globals.Core.Handlers.DocumentUpload
{
    /// <summary>
    /// Handler class to upload documents to azure blob
    /// </summary>
    public class DocumentUploadHandler : IRequestHandler<DocumentUploadRequest, Dictionary<string, string>>
    {
        private readonly IStorageManager storageManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentUploadHandler"/> class.
        /// </summary>
        /// <param name="storageManager">object of storage manager to handle azure blob operation</param>
        public DocumentUploadHandler(IStorageManager storageManager)
        {
            this.storageManager = storageManager;
        }

        /// <inheritdoc />
        public async Task<Dictionary<string, string>> Handle(DocumentUploadRequest request, CancellationToken cancellationToken)
        {
            if (request == null || request.Documents == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Documents.Count() != request.Metadatas.Count())
            {
                throw new ArgumentException(nameof(request));
            }

            byte[] fileAsByte = null;
            int counter = 0;
            var response = new Dictionary<string, string>();
            foreach (var document in request.Documents)
            {
                if (document.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        document.CopyTo(ms);
                        fileAsByte = ms.ToArray();
                    }
                }
                else
                {
                    throw new TQException("File is empty");
                }

                Dictionary<string, string> metadata = new Dictionary<string, string>();
                if (request.Metadatas.ElementAt(counter) != null && request.Metadatas.ElementAt(counter).Any())
                {
                    var metaSize = 0;
                    const int maxMetaSize = 8000;
                    foreach (var meta in request.Metadatas.ElementAt(counter))
                    {
                        var size = 0;
                        size += ASCIIEncoding.Unicode.GetBytes(meta.Key).Length;
                        size += ASCIIEncoding.Unicode.GetBytes(meta.Value).Length;
                        if (!string.IsNullOrEmpty(meta.Value) && metaSize + size < maxMetaSize)
                        {
                            metaSize += size;
                            metadata.Add(meta.Key, meta.Value);
                        }
                    }
                }
                List<string> documentPathList = new List<string>();

                if (!string.IsNullOrEmpty(request.SubContainerName))
                {
                    documentPathList.Add(request.SubContainerName);
                }

                documentPathList.Add(document?.FileName);

                var destinationPath = string.Join("/", documentPathList);
                var containerExist = await storageManager.ContainerExistsAsync(request.ContainerName.ToLower()).ConfigureAwait(false);
                if (!containerExist)
                {
                    throw new TQException("Container does not exist");
                }

                var fileUri = await storageManager.UploadFileToBlobStorageAsync(request.ContainerName.ToLower(), destinationPath, fileAsByte, document.ContentType, metadata).ConfigureAwait(false);
                response.Add(document.FileName, fileUri);
                counter++;
            }
            return response;
        }
    }
}