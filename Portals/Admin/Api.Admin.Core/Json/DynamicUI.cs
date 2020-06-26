namespace Api.Admin.Core.Json
{
    /// <summary>
    /// A Model class to request Dynamic UI
    /// </summary>
    public class DynamicUI
    {
        /// <summary>
        /// Gets or Sets the Domain name
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or Sets the Country name
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or Sets the template Url
        /// </summary>
        public string TemplateUrl { get; set; }
    }
}