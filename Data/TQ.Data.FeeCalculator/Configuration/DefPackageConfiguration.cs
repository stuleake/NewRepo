using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Globalization;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Definition Package Configuration for seed data.
    /// </summary>
    public class DefPackageConfiguration : IEntityTypeConfiguration<DefPackage>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<DefPackage> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("DefPackages", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new DefPackage
                {
                    DefPackageId = GuidConstants.DraftDefPackageGuid,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = GuidConstants.TestUserGuid,
                    FileName = "Test_Package_20200504",
                    Status = "Draft"
                },
                new DefPackage
                {
                    DefPackageId = GuidConstants.LiveDefPackageGuid,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = GuidConstants.TestUserGuid,
                    FileName = "Test_Package_20200503",
                    Status = "Live"
                });
        }
    }
}