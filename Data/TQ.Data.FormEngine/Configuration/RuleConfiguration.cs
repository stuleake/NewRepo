using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;
using TQ.Core.Enums;
using TQ.Data.FormEngine.Schemas.Forms;

namespace TQ.Data.FormEngine.Configuration
{
    /// <summary>
    /// Rule Configuration for Seed data.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RuleConfiguration : IEntityTypeConfiguration<Rule>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Rule> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Rules", FormEngineSchemas.Forms);
            foreach (Rules Rule in Enum.GetValues(typeof(Rules)))
            {
                builder.HasData(
                    new Rule
                    {
                        RuleId = (int)Rule,
                        Rules = Rule.ToString()
                    });
            }
        }
    }
}
