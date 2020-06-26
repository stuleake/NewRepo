using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Telerik.JustMock;

namespace UnitTest.Helpers.FakeClients
{
    /// <summary>
    /// Fake sendgrid client
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeSendgridClient
    {
        /// <summary>
        /// Gets the sendgrid client
        /// </summary>
        public ISendGridClient SendgridClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeSendgridClient"/> class.
        /// </summary>
        public FakeSendgridClient()
        {
            SendgridClient = Mock.Create<ISendGridClient>();

            Mock.Arrange(() => SendgridClient.SendEmailAsync(
                Arg.IsAny<SendGridMessage>(),
                Arg.IsAny<CancellationToken>())).Returns((SendGridMessage msg, CancellationToken cancellationToken) => SendEmailAsync());
        }

        private async Task<Response> SendEmailAsync()
        {
            var statusCode = default(HttpStatusCode);
            HttpContent responseBody = null;
            HttpResponseHeaders headers = null;
            var result = new Response(statusCode, responseBody, headers) { StatusCode = HttpStatusCode.OK };

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}