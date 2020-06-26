using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Rule Definition and Parameter Mapping Configuration for seed data.
    /// </summary>
    public class RuleDefParameterMappingConfiguration : IEntityTypeConfiguration<RuleDefParameterMapping>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<RuleDefParameterMapping> builder)
        {
            const string boolType = "bool";
            const string numberType = "number";
            const string intType = "int";

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("RuleDefParameterMappings", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("87c47b82-019a-4741-8c20-1616480701a4"),
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid,
                    ParameterId = GuidConstants.IsResidentialParameterGuid,
                    Sequence = 1,
                    ParameterType = boolType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("ec4578df-b012-46e9-b28a-fc06ba7e53d5"),
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid,
                    ParameterId = GuidConstants.IsEnglargementImprovementAlterationParameterGuid,
                    Sequence = 2,
                    ParameterType = boolType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("4159ab87-86e7-46ed-bee5-185d59e48192"),
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid,
                    ParameterId = GuidConstants.IsEnlargementImprovementAlterationSingleParameterGuid,
                    Sequence = 3,
                    ParameterType = boolType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("b36f3a35-14e8-479d-be3a-77292ddff866"),
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid,
                    ParameterId = GuidConstants.IsEnlargementImprovementAlterationMultipleParameterGuid,
                    Sequence = 4,
                    ParameterType = boolType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("b99ac924-a5b8-4d1e-8c42-9f16c420ceef"),
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid,
                    ParameterId = GuidConstants.EnlargementImprovementAlterationSingleChargeParameterGuid,
                    Sequence = 5,
                    ParameterType = numberType,
                    OutputOperation = "Add",
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("14b88f7c-4646-46d5-96ad-0b2bc60da853"),
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid,
                    ParameterId = GuidConstants.EnlargementImprovementAlterationMultipleChargeParameterGuid,
                    Sequence = 6,
                    ParameterType = numberType,
                    OutputOperation = "Add",
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("3d25792f-5bd8-4302-8b3d-4c2da5f85464"),
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid,
                    ParameterId = GuidConstants.OtherMixedDevelopmentFeeParameterGuid,
                    Sequence = 7,
                    ParameterType = numberType,
                    OutputOperation = "Add",
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("d443c0a2-c52f-46c9-8c48-9f42f9e30bd2"),
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid,
                    ParameterId = GuidConstants.IsNewDwellingsParameterGuid,
                    Sequence = 8,
                    ParameterType = boolType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("10b20383-a88a-488a-839c-41c49b8b298c"),
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid,
                    ParameterId = GuidConstants.NumNewDwellingsParameterGuid,
                    Sequence = 9,
                    ParameterType = intType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("459670f9-0c3c-4f82-b1a6-a3c6780dff0e"),
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid,
                    ParameterId = GuidConstants.NewDwellingChargeParameterGuid,
                    Sequence = 10,
                    ParameterType = numberType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("1b4fd17c-8003-4ae6-b063-042160742c64"),
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid,
                    ParameterId = GuidConstants.NewDwellingFlatChargeParameterGuid,
                    Sequence = 11,
                    ParameterType = numberType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("bc8aa501-5628-450d-a8fe-cb7d7e919880"),
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid,
                    ParameterId = GuidConstants.AdditionalNewDwellingChargeParameterGuid,
                    Sequence = 12,
                    ParameterType = numberType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("665fa40d-7502-4b81-99ee-e5659675593f"),
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid,
                    ParameterId = GuidConstants.MaxNewDwellingChargeParameterGuid,
                    Sequence = 13,
                    ParameterType = numberType,
                    OutputOperation = string.Empty,
                    IsFinalOutput = false
                },
                new RuleDefParameterMapping
                {
                    RuleDefParameterMappingId = Guid.Parse("55085277-bfd6-492c-be77-6fdf072e4f76"),
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid,
                    ParameterId = GuidConstants.ResidentialFeeParameterGuid,
                    Sequence = 14,
                    ParameterType = numberType,
                    OutputOperation = "Add",
                    IsFinalOutput = false
                });
        }
    }
}