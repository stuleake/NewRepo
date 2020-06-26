using Api.Admin.Core.Handlers.FeeCalculator;
using Api.Admin.Core.Services.FeeCalculator;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using Xunit;

namespace UnitTest.Api.Admin.Core.Handlers.FeeCalculator
{
    [ExcludeFromCodeCoverage]
    public class ImportFeeCalcRulesHandlerTests
    {
        private readonly ImportFeeCalcRulesHandler sut;
        private readonly System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
        private readonly IFeeCalculatorClient mockFeeCalculatorClient = Mock.Create<IFeeCalculatorClient>();
        private readonly IConfiguration mockConfiguration = Mock.Create<IConfiguration>();

        public ImportFeeCalcRulesHandlerTests()
        {
            this.sut = new ImportFeeCalcRulesHandler(mockConfiguration, mockFeeCalculatorClient);
        }

        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }
    }
}