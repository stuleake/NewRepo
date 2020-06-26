using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Session Type Configuration for seed data.
    /// </summary>
    public class SessionTypeConfiguration : IEntityTypeConfiguration<SessionType>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<SessionType> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("SessionTypes", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new SessionType
                {
                    SessionTypeId = Guid.Parse("1145d244-fea5-45b0-9927-9c21dcd34566"),
                    Name = "TestSet"
                },
                new SessionType
                {
                    SessionTypeId = Guid.Parse("024512ec-6b91-4165-bb6a-0cfe9dc0965c"),
                    Name = "QSR"
                });
        }
    }
}