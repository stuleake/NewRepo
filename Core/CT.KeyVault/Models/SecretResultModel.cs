namespace CT.KeyVault.Models
{
    /// <summary>
    /// KeyVault secret value result model
    /// </summary>
    public sealed class SecretResultModel
    {
        /// <summary>
        /// Gets or sets the id of the secret
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the secret
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the vale of the secret
        /// </summary>
        public string Value { get; set; }
    }
}