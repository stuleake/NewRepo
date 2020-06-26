using CT.Storage.Enum;

namespace CT.Storage
{
    /// <summary>
    /// Blob storage connection factory
    /// </summary>
    public static class StorageProvider
    {
        /// <summary>
        /// Provides the blob storage manager
        /// </summary>
        /// <param name="storageconnection">Connection string for the blob storage</param>
        /// <param name="connectionType">connection type</param>
        /// <param name="sasexpiry">SAS token expiry</param>
        /// <returns>A <see cref="IStorageManager"/> repeseenting the blob conneciton object></returns>
        public static IStorageManager CreateManager(string storageconnection, ConnectionTypes connectionType, int sasexpiry = 0)
        {
            return new BlobManager(storageconnection, connectionType, sasexpiry);
        }
    }
}