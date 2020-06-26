using Api.FeeCalculator.Core.Commands.FeeCalculator;
using Api.FeeCalculator.Core.Handlers.FeeCalculator;
using Api.FeeCalculator.Core.Helpers.JSEngine;
using Api.FeeCalculator.Core.ViewModels;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Enums;
using TQ.Data.FeeCalculator;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FeeCalculator.Core
{
    /// <summary>
    /// Unit test for Fee calculator
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FeeCalculatorHandlerTest
    {
        private readonly IMapper mapper;
        private readonly FeeCalculatorContext feeCalculatorContext;
        private readonly JsEngine jsengine;
        private readonly Guid categoryId;
        private readonly Guid ruleDefId;
        private readonly Guid boolDataType;
        private readonly Guid numberDataType;
        private readonly Guid defPackageId;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeCalculatorHandlerTest"/> class.
        /// </summary>
        public FeeCalculatorHandlerTest()
        {
            feeCalculatorContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FeeCalculatorContext>();
            mapper = UnitTestHelper.GiveServiceProvider().GetRequiredService<IMapper>();
            jsengine = UnitTestHelper.GiveServiceProvider().GetRequiredService<JsEngine>();
            categoryId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6335");
            boolDataType = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6336");
            numberDataType = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6337");
            defPackageId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6338");
            ruleDefId = Guid.Parse("6EBE528C-31D6-4ACB-AF6A-9EB67AFE6338");
            feeCalculatorContext.DefPackages.Add(new TQ.Data.FeeCalculator.Schemas.Dbo.DefPackage
            {
                DefPackageId = defPackageId,
                CreatedBy = Guid.Empty,
                CreatedDate = DateTime.Now,
                FileName = "Unit Test",
                Status = RuleDefPackageStatusConstants.Draft
            });
            feeCalculatorContext.Categories.Add(new TQ.Data.FeeCalculator.Schemas.Dbo.Category
            {
                CategoryId = categoryId,
                CategoryName = "FeeRules",
                Sequence = 1
            });
            feeCalculatorContext.ParamDataTypes.Add(new TQ.Data.FeeCalculator.Schemas.Dbo.ParamDataType
            {
                ParamDataTypeId = boolDataType,
                ParamDataTypeName = "bool"
            });
            feeCalculatorContext.ParamDataTypes.Add(new TQ.Data.FeeCalculator.Schemas.Dbo.ParamDataType
            {
                ParamDataTypeId = numberDataType,
                ParamDataTypeName = "number"
            });
            feeCalculatorContext.RuleDefs.Add(new TQ.Data.FeeCalculator.Schemas.Dbo.RuleDef
            {
                RuleDefId = ruleDefId,
                CategoryId = categoryId,
                CreatedBy = Guid.Empty,
                CreatedDate = DateTime.UtcNow,
                DefPackageId = defPackageId,
                Tenant = CountryConstants.England,
                Product = "fee",
                StartDate = DateTime.ParseExact("01/01/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                ReferenceId = 1,
                RuleDefinition = "if(Is_newDwelling && !isNaN(NewDwellingFlatFee) && !isNaN(NumberOfNewDwellings)){Fee = NewDwellingFlatFee * NumberOfNewDwellings;" +
                "desc = \"new DwellingFee is \" + Fee;var output = {\"description\": desc,\"outputparametervalue\": Fee};return JSON.stringify(output)}",
                RuleName = "Test Rule"
            });
            var is_newDwelling = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6339");
            var newDwellingFlatFee = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6340");
            var numberOfNewDwellings = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6341");
            var fee = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6342");
            feeCalculatorContext.Parameters.AddRange(
                new List<TQ.Data.FeeCalculator.Schemas.Dbo.Parameter>
                {
                    new TQ.Data.FeeCalculator.Schemas.Dbo.Parameter
                    {
                        ParameterId = is_newDwelling,
                        ParamDataTypeId = boolDataType,
                        Name = "Is_newDwelling",
                        Product = "fee",
                        Tenant = CountryConstants.England
                    },
                    new TQ.Data.FeeCalculator.Schemas.Dbo.Parameter
                    {
                        ParameterId = newDwellingFlatFee,
                        ParamDataTypeId = numberDataType,
                        Name = "NewDwellingFlatFee",
                        Product = "fee",
                        Tenant = CountryConstants.England,
                        MasterValue = "1250"
                    },
                    new TQ.Data.FeeCalculator.Schemas.Dbo.Parameter
                    {
                        ParameterId = numberOfNewDwellings,
                        ParamDataTypeId = numberDataType,
                        Name = "NumberOfNewDwellings",
                        Product = "fee",
                        Tenant = CountryConstants.England
                    },
                    new TQ.Data.FeeCalculator.Schemas.Dbo.Parameter
                    {
                        ParameterId = fee,
                        ParamDataTypeId = numberDataType,
                        Name = "Fee",
                        Product = "fee",
                        Tenant = CountryConstants.England
                    }
                });
            feeCalculatorContext.RuleDefParameterMappings.AddRange(new List<TQ.Data.FeeCalculator.Schemas.Dbo.RuleDefParameterMapping>
            {
                new TQ.Data.FeeCalculator.Schemas.Dbo.RuleDefParameterMapping
                {
                RuleDefParameterMappingId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6343"),
                RuleDefId = ruleDefId,
                ParameterId = is_newDwelling,
                ParameterType = ParameterTypes.In.ToString(),
                Sequence = 0,
                },
                new TQ.Data.FeeCalculator.Schemas.Dbo.RuleDefParameterMapping
                {
                RuleDefParameterMappingId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6345"),
                RuleDefId = ruleDefId,
                ParameterId = newDwellingFlatFee,
                ParameterType = ParameterTypes.In.ToString(),
                Sequence = 1,
                },
                new TQ.Data.FeeCalculator.Schemas.Dbo.RuleDefParameterMapping
                {
                RuleDefParameterMappingId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6346"),
                RuleDefId = ruleDefId,
                ParameterId = numberOfNewDwellings,
                ParameterType = ParameterTypes.In.ToString(),
                Sequence = 2,
                },
                new TQ.Data.FeeCalculator.Schemas.Dbo.RuleDefParameterMapping
                {
                RuleDefParameterMappingId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6347"),
                RuleDefId = ruleDefId,
                ParameterId = fee,
                ParameterType = ParameterTypes.Out.ToString(),
                Sequence = 3,
                },
            });
            feeCalculatorContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Test the handler to Calculate the fee for given input data
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Fact]
        public async Task Handle_FeeCalculatorAsync()
        {
            FeeCalculatorHandler handler = new FeeCalculatorHandler(feeCalculatorContext, jsengine, mapper);
            FeeCalculatorData request = new FeeCalculatorData
            {
                SessionType = "Qsr",
                SessionId = Guid.Parse("3ECE37D2-F304-465F-7284-08D7AABDDD69"),
                QsCollectionId = Guid.Parse("079891D1-E796-48AB-1FA3-08D7A62B727F"),
                Country = "England",
                Language = "English",
                Region = "England",
                Answers = new List<AnswerModel>
                {
                    new AnswerModel
                    {
                        ParameterName = "Is_newDwelling",
                        Datatype = "bool",
                        Answer = "true"
                    },
                    new AnswerModel
                    {
                        ParameterName = "NumberOfNewDwellings",
                        Datatype = "number",
                        Answer = "2"
                    }
                }
            };
            var response = await handler.Handle(request, System.Threading.CancellationToken.None);
            Assert.NotNull(response);
            Assert.Single(response.CalculationSteps.ToList());
            Assert.Equal("2500", response.CalculationSteps.First().Output);
        }
    }
}