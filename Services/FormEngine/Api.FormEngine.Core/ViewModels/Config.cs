using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels
{
    /// <summary>
    /// Model class to keep vault secrets
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets or Sets the secrets
        /// </summary>
        public Dictionary<string, string> Secrets { get; set; }
    }
}