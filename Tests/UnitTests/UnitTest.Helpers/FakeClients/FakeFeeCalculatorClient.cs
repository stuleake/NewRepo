using Api.Admin.Core.Services.FeeCalculator;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Models;

namespace UnitTest.Helpers.FakeClients
{
    /// <summary>
    /// Fake Fee Calculator
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeFeeCalculatorClient
    {
        /// <summary>
        /// Gets the fee calculator client
        /// </summary>
        public IFeeCalculatorClient FeeCalculatorClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeFeeCalculatorClient"/> class.
        /// </summary>
        public FakeFeeCalculatorClient()
        {
            FeeCalculatorClient = Mock.Create<IFeeCalculatorClient>();

            Mock.Arrange(() => FeeCalculatorClient.ImportFeeCalculatorRulesAsync(Arg.IsAny<string>(), Arg.IsAny<JObject>(), Arg.IsAny<string>(), Arg.IsAny<string>()))
                .Returns((string requestUrl, JObject data, string country, string authToken) => ImportFeeCalculatorRulesAsync());
        }

        private async Task<ServiceResponse<bool>> ImportFeeCalculatorRulesAsync()
        {
            var result = new ServiceResponse<bool>
            {
                Code = 201,
                Value = true
            };

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}