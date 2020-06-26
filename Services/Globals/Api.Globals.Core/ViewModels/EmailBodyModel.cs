namespace Api.Globals.Core.ViewModels
{
    /// <summary>
    /// Email body for sendgrid template
    /// </summary>
    public class EmailBodyModel
    {
        /// <summary>
        /// Gets or sets name for the email
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets link for email
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets subject for email
        /// </summary>
        public string Subject { get; set; }
    }
}