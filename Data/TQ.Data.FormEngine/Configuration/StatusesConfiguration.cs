using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FormEngine.Schemas.Forms;

namespace TQ.Data.FormEngine.Configuration
{
    /// <summary>
    /// Statuses Configuration for Seed data.
    /// </summary>
    public class StatusesConfiguration : IEntityTypeConfiguration<Statuses>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Statuses> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Status", FormEngineSchemas.Forms);

            // TODO: convert to enum
            builder.HasData(
                new Statuses
                {
                    StatusId = 1,
                    Status = "Draft"
                },
                new Statuses
                {
                    StatusId = 2,
                    Status = "Active"
                },
                new Statuses
                {
                    StatusId = 3,
                    Status = "Legacy"
                });
        }
    }
}