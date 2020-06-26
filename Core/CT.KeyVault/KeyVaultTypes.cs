namespace CT.KeyVault
{
    /// <summary>
    /// Supported connection types
    /// </summary>
    public enum KeyVaultTypes
    {
        /// <summary>
        /// via Managed Service Identity
        /// </summary>
        MSI,

        /// <summary>
        /// via Application permission
        /// </summary>
        Application,

        /// <summary>
        /// via Service Principal
        /// </summary>
        ServicePrincipal
    }
}