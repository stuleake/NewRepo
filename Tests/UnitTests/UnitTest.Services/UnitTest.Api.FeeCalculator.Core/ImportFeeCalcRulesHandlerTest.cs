using Api.FeeCalculator.Core.Commands.FeeCalculator;
using Api.FeeCalculator.Core.Handlers.FeeCalculator;
using Api.FeeCalculator.Core.ViewModels;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Data.FeeCalculator;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FeeCalculator.Core
{
    /// <summary>
    /// Unit test for import fee calc rules
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ImportFeeCalcRulesHandlerTest
    {
        private readonly IMapper mapper;
        private readonly FeeCalculatorContext feeCalculatorContext;
        private readonly Guid categoryId;
        private readonly Guid datatypeId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportFeeCalcRulesHandlerTest"/> class.
        /// </summary>
        public ImportFeeCalcRulesHandlerTest()
        {
            feeCalculatorContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FeeCalculatorContext>();
            mapper = UnitTestHelper.GiveServiceProvider().GetRequiredService<IMapper>();
            datatypeId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6335");
            feeCalculatorContext.ParamDataTypes.Add(new TQ.Data.FeeCalculator.Schemas.Dbo.ParamDataType
            {
                ParamDataTypeId = datatypeId,
                ParamDataTypeName = "bool"
            });
            categoryId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6334");
            feeCalculatorContext.Categories.Add(new TQ.Data.FeeCalculator.Schemas.Dbo.Category
            {
                CategoryId = categoryId,
                CategoryName = "FeeRules",
                Sequence = 2
            });
            feeCalculatorContext.Parameters.Add(new TQ.Data.FeeCalculator.Schemas.Dbo.Parameter
            {
                Name = "Is_NewDwellings",
                ParamDataTypeId = datatypeId,
                Product = "Fee",
                Tenant = CountryConstants.England
            });
            feeCalculatorContext.SaveChanges();
        }

        /// <summary>
        /// Test the handler to save fee calculator rules to database
        /// </summary>
        /// <returns>true if success</returns>
        [Fact]
        public async Task Handle_ImportFeeCalcRulesAsync()
        {
            FeeCalculatorRules request = new FeeCalculatorRules
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Country = CountryConstants.England,
                FileName = "FeeCalc_IntegrationTest.zip",
                Rules = new List<RuleDefModel>
                {
                    new RuleDefModel
                    {
                        RuleId = 1,
                        RuleName = "Test Rule Name",
                        RuleDefinition = "Test Definition",
                        Category = "FeeRules",
                        StartDate = DateTime.ParseExact("01/01/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        EndDate = null,
                        Parameters = new List<ParameterModel>
                        {
                            new ParameterModel
                            {
                                Name = "Is_Residential",
                                Datatype = "bool",
                                ParameterType = "in"
                            },
                            new ParameterModel
                            {
                                Name = "Is_NewDwellings",
                                Datatype = "bool",
                                ParameterType = "out"
                            }
                        }
                    }
                },
            };

            ImportFeeCalcRulesHandler handler = new ImportFeeCalcRulesHandler(feeCalculatorContext, mapper);
            var response = await handler.Handle(request, System.Threading.CancellationToken.None);

            Assert.True(response);
        }
    }
}