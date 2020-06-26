using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Globalization;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Test Set Header Configuration for seed data.
    /// </summary>
    public class TestSetHeaderConfiguration : IEntityTypeConfiguration<TestSetHeader>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<TestSetHeader> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("TestSetHeaders", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new TestSetHeader
                {
                    TestSetHeaderId = Guid.Parse("cdd6da23-380d-4a70-8b34-c8a0d61417c3"),
                    FileName = "Test Set 1",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = GuidConstants.TestUserGuid
                },
                new TestSetHeader
                {
                    TestSetHeaderId = Guid.Parse("69c08242-2666-4dcd-a2f6-6874ac875aa8"),
                    FileName = "Test Set 2",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = GuidConstants.TestUserGuid
                });
        }
    }
}