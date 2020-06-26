using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Question Set Collection (application) Type Rule Mapping Configuration for seed data.
    /// </summary>
    public class QSCollectionTypeRuleMappingConfiguration : IEntityTypeConfiguration<QSCollectionTypeRuleMapping>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<QSCollectionTypeRuleMapping> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("QSCollectionTypeRuleMappings", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new QSCollectionTypeRuleMapping
                {
                    QSCollectionTypeRuleMappingId = Guid.Parse("5369b12a-e07a-418d-af22-f8e1e6bca9e7"),
                    ApplicationTypeRefNo = 1,
                    QSCollectionVersion = "1.0",
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid
                },
                new QSCollectionTypeRuleMapping
                {
                    QSCollectionTypeRuleMappingId = Guid.Parse("f501fb97-4936-4635-a998-d715069e42cd"),
                    ApplicationTypeRefNo = 2,
                    QSCollectionVersion = "1.0",
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid,
                });
        }
    }
}