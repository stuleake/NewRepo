using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Definition Package Configuration for seed data.
    /// </summary>
    public class StepConfiguration : IEntityTypeConfiguration<Step>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Step> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Steps", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new Step
                {
                    StepId = Guid.Parse("92d9e4e7-7022-4676-8a8a-f25a6f84ce99"),
                    RuleId = GuidConstants.ResidentialDwellingAlterationRuleGuid,
                    SessionId = GuidConstants.UserOneSessionGuid,
                    Description = "Fee Rule 01 Residential dwellings alterations",
                    CategoryId = GuidConstants.FeeRulesCategoryGuid,
                    ApplicationTypeRefId = 1,
                    RuleNo = 1,
                    Inputs = "Is_Residential, Is_EnlargementImprovementAlteration, " +
                                "Is_EnlargementImprovementAlteration_Single, " +
                                "Is_EnlargementImprovementAlteration_Multiple, " +
                                "EnlargementImprovementAlterationSingleCharge, " +
                                "EnlargementImprovementAlterationMultipleCharge,",
                    Output = "OtherMixedDevelopmentFee",
                    OutputParamName = "OtherMixedDevelopmentFee",
                    OutputDataType = "number",
                    IsFinalOutput = false
                },
                new Step
                {
                    StepId = Guid.Parse("add1ef8a-c30d-4555-9c37-b6bfc94b3e67"),
                    RuleId = GuidConstants.CreateNewDwellingRuleGuid,
                    SessionId = GuidConstants.UserOneSessionGuid,
                    Description = "Fee Rule 02 Create New Dwellings",
                    CategoryId = GuidConstants.FeeRulesCategoryGuid,
                    ApplicationTypeRefId = 1,
                    RuleNo = 2,
                    Inputs = "Is_NewDwellings, Num_NewDwellings, NewDwellingCharge, " +
                             "NewDwellingFlatCharge, AdditionalNewDwellingCharge, MaxNewDwellingCharge",
                    Output = "ResidentialFee",
                    OutputParamName = "ResidentialFee",
                    OutputDataType = "number",
                    IsFinalOutput = false
                });
        }
    }
}