using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace CT.Storage.Helpers
{
    /// <summary>
    /// Helper class to provide resource permissins
    /// </summary>
    public static class BlobPersmissionHelper
    {
        /// <summary>
        /// Converts string permissions to the permission flags
        /// </summary>
        /// <param name="permissions">string permissions for accessing the storage</param>
        /// <returns>A <see cref="SharedAccessBlobPermissions"/> object that relates to the permissions</returns>
        public static SharedAccessBlobPermissions GetPersmissions(string permissions)
        {
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }
            SharedAccessBlobPermissions tempPermission = default(SharedAccessBlobPermissions);
            var perm = permissions.Split('|');
            foreach (var permission in perm)
            {
                switch (permission.ToLower().Trim())
                {
                    case "none":
                        tempPermission |= SharedAccessBlobPermissions.None;
                        break;

                    case "read":
                        tempPermission |= SharedAccessBlobPermissions.Read;
                        break;

                    case "write":
                        tempPermission |= SharedAccessBlobPermissions.Write;
                        break;

                    case "list":
                        tempPermission |= SharedAccessBlobPermissions.List;
                        break;

                    case "delete":
                        tempPermission |= SharedAccessBlobPermissions.Delete;
                        break;

                    case "create":
                        tempPermission |= SharedAccessBlobPermissions.Create;
                        break;

                    case "add":
                        tempPermission |= SharedAccessBlobPermissions.Add;
                        break;

                    default: throw new NotSupportedException("Permission Not Supported");
                }
            }
            return tempPermission;
        }
    }
}