using Api.Admin.Core.Commands.FeeCalculator;
using Api.Admin.Core.Handlers.FeeCalculator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using UnitTest.Helpers.FakeClients;
using Xunit;

namespace UnitTest.Api.Admin.Core
{
    public class ImportFeeCalcHandlerTest
    {
        private readonly IConfigurationRoot config;
        private readonly FakeFeeCalculatorClient fakeFeeCalculatorClient;

        public ImportFeeCalcHandlerTest()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            fakeFeeCalculatorClient = new FakeFeeCalculatorClient();
        }

        /// <summary>
        /// Test the zip file parsing having correct format
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ImportFeeCalcHandler()
        {
            // Arrange
            var fileName = config["FeeCalculator:FileNames:ImportFileName"];
            string path = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), fileName);

            byte[] fileArray = File.ReadAllBytes(path);
            IFormFile formFileDocument = new FormFile(new MemoryStream(fileArray), 0, fileArray.Length, "Document", fileName)
            {
                Headers = new HeaderDictionary(),
            };
            ImportFeeCalculationRules request = new ImportFeeCalculationRules
            {
                File = formFileDocument,
                Country = "england",
                UserId = Guid.Parse("912BC7C3-5376-40B5-A064-7CBA5D243CDE")
            };

            // Act
            ImportFeeCalcRulesHandler handler = new ImportFeeCalcRulesHandler(config, fakeFeeCalculatorClient.FeeCalculatorClient);

            var response = await handler.Handle(request, System.Threading.CancellationToken.None);

            // Assert
            Assert.True(response);
        }

        /// <summary>
        /// Tests the parsing of zip file having invalid rule data
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_TQExceptionForInvalidRule()
        {
            // Arrange
            var fileName = config["FeeCalculator:FileNames:FileWithInvalidRule"];
            string path = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), fileName);

            byte[] fileArray = File.ReadAllBytes(path);
            string file = Path.GetFileName(path);
            IFormFile formFileDocument = new FormFile(new MemoryStream(fileArray), 0, fileArray.Length, "Document", fileName)
            {
                Headers = new HeaderDictionary(),
            };
            ImportFeeCalculationRules request = new ImportFeeCalculationRules
            {
                File = formFileDocument,
                Country = "england",
                UserId = Guid.Parse("912BC7C3-5376-40B5-A064-7CBA5D243CDE")
            };

            // Act
            ImportFeeCalcRulesHandler handler = new ImportFeeCalcRulesHandler(config, fakeFeeCalculatorClient.FeeCalculatorClient);

            var exception = await Assert.ThrowsAsync<TQException>(() => handler.Handle(request, System.Threading.CancellationToken.None));

            // Assert
            Assert.Equal("Invalid content in Rule file with Name : Rule_125.json", exception.Message);
        }

        /// <summary>
        /// Tests import fee calculator rules handler with null request object
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_NullRequest()
        {
            // Act
            var expectedErrorMessage = "Value cannot be null. (Parameter 'request')";
            ImportFeeCalcRulesHandler handler = new ImportFeeCalcRulesHandler(config, fakeFeeCalculatorClient.FeeCalculatorClient);
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, System.Threading.CancellationToken.None));

            // Assert
            Assert.Equal(expectedErrorMessage, exception.Message);
        }
    }
}