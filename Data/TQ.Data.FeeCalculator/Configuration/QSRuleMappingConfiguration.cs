using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Question Set (Form) Rule Mapping Configuration for seed data.
    /// </summary>
    public class QSRuleMappingConfiguration : IEntityTypeConfiguration<QSRuleMapping>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<QSRuleMapping> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("QSRuleMapping", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new QSRuleMapping
                {
                    QSRuleMappingId = Guid.Parse("f1e6a032-3b78-4a10-bd35-7157556a422e"),
                    QSNo = 1,
                    QSVersion = "1.0",
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid
                },
                new QSRuleMapping
                {
                    QSRuleMappingId = Guid.Parse("791388a4-cf41-44f6-b435-4a85305f84ee"),
                    QSNo = 2,
                    QSVersion = "1.0",
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid
                });
        }
    }
}