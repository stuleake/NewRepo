using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TQ.Core.Enums;
using TQ.Data.FormEngine.Schemas.Forms;

namespace TQ.Data.FormEngine.Configuration
{
    /// <summary>
    /// Displays Configuration for Seed data.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DisplaysConfiguration : IEntityTypeConfiguration<Display>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Display> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Displays", FormEngineSchemas.Forms);

            foreach (DisplayTypes display in Enum.GetValues(typeof(DisplayTypes)))
            {
                builder.HasData(
                    new Display
                    {
                        DisplayId = (int)display,
                        Displays = display.ToString()
                    });
            }
        }
    }
}