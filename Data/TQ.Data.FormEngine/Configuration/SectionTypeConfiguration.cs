using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FormEngine.Schemas.Forms;

namespace TQ.Data.FormEngine.Configuration
{
    /// <summary>
    /// SectionType Configuration for Seed data.
    /// </summary>
    public class SectionTypeConfiguration : IEntityTypeConfiguration<SectionType>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<SectionType> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("SectionTypes", FormEngineSchemas.Forms);

            // TODO: convert to enum
            builder.HasData(
                new SectionType
                {
                    SectionTypeId = 1,
                    SectionTypes = "Main-Fields"
                },
                new SectionType
                {
                    SectionTypeId = 2,
                    SectionTypes = "Main-Table"
                },
                new SectionType
                {
                    SectionTypeId = 3,
                    SectionTypes = "Sub-Fields"
                },
                new SectionType
                {
                    SectionTypeId = 4,
                    SectionTypes = "Sub-Table"
                });
        }
    }
}