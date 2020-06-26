using Api.Globals.Core.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Globals.Core.Helpers
{
    /// <summary>
    /// Class for Email service
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Gets or Sets HTTP client wrapper for interacting with Twilio SendGrid's API
        /// </summary>
        public ISendGridClient SendgridClient { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class
        /// </summary>
        /// <param name="appkey">appkey being passed using dependency injection</param>
        public EmailService(string appkey)
        {
            this.SendgridClient = new SendGridClient(appkey);
        }

        /// <summary>
        /// Send emails via sendgrid
        /// </summary>
        /// <param name="emailid">Recipient mail-id</param>
        /// <param name="emailBody">Body of the mail</param>
        /// <param name="from">Sender mail-id</param>
        /// <param name="templateid">TemplateId</param>
        /// <param name="emailidCC">CC List</param>
        /// <param name="bccEmailid">BCC List</param>
        /// <returns>Returns bool value status for SendEmail</returns>
        public async Task<bool> SendEmailAsync(string emailid, EmailBodyModel emailBody, string from, string templateid, string emailidCC = null, string bccEmailid = null)
        {
            if (string.IsNullOrEmpty(emailid))
            {
                throw new ArgumentNullException(nameof(emailid));
            }
            try
            {
                // send an email message using the SendGrid Web API
                var msg = new SendGridMessage();
                msg.SetFrom(from);

                // have multiple reciepents to send mail
                if (emailid.Contains(";", StringComparison.InvariantCulture))
                {
                    var emaillist = emailid.Split(';').Select(x => x).ToList();
                    foreach (var email in emaillist)
                    {
                        msg.AddTo(email);
                    }
                }
                else
                {
                    msg.AddTo(emailid);
                }

                // send email to CC mailid
                if (!string.IsNullOrEmpty(emailidCC))
                {
                    msg.AddCc(emailidCC);
                }

                // send email to BCC mailid
                if (!string.IsNullOrEmpty(bccEmailid))
                {
                    msg.AddBcc(bccEmailid);
                }

                // Pass the email template id.
                msg.SetTemplateId(templateid);

                // pass the email template data to be replaced in template
                msg.SetTemplateData(emailBody);
                var response = await SendgridClient.SendEmailAsync(msg).ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}