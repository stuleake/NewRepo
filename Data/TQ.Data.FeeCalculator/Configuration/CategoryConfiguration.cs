using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Category Type Configuration for seed data.
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Categories", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new Category
                {
                    CategoryId = GuidConstants.FeeRulesCategoryGuid,
                    CategoryName = "FeeRules",
                    Sequence = 1
                },
                new Category
                {
                    CategoryId = GuidConstants.ConcessionRulesCategoryGuid,
                    CategoryName = "ConcessionRules",
                    Sequence = 2
                },
                new Category
                {
                    CategoryId = GuidConstants.ServiceChargeRulesCategoryGuid,
                    CategoryName = "ServiceChargeRules",
                    Sequence = 3
                });
        }
    }
}