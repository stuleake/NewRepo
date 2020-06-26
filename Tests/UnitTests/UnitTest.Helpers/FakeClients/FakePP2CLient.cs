using Api.Planner.Core.Services.PP2;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Models;

namespace UnitTest.Helpers.FakeClients
{
    /// <summary>
    /// Fake PP2 Client
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakePP2CLient
    {
        /// <summary>
        /// Gets the client
        /// </summary>
        public IPP2HttpClient Pp2HttpClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakePP2CLient"/> class.
        /// </summary>
        public FakePP2CLient()
        {
            Pp2HttpClient = Mock.Create<IPP2HttpClient>();

            Mock.Arrange(() => Pp2HttpClient.CreateQuestionSetResponseAsync(Arg.IsAny<string>(), Arg.IsAny<JObject>(), Arg.IsAny<string>(), Arg.IsAny<string>()))
                .Returns((string uri, JObject obj, string authToken, string country) => CreateQuestionSetResponseAsync());
        }

        private async Task<ServiceResponse<Guid>> CreateQuestionSetResponseAsync()
        {
            var result = new ServiceResponse<Guid> { Code = 201, Value = Guid.NewGuid() };
            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}