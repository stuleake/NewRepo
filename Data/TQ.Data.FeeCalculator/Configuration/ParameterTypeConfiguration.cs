using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Parameter Type Configuration for seed data.
    /// </summary>
    public class ParameterTypeConfiguration : IEntityTypeConfiguration<ParameterType>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<ParameterType> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("ParameterTypes", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new ParameterType
                {
                    ParameterTypeId = Guid.Parse("6c6b863f-728f-444c-802c-d1b66d92d7f3"),
                    Name = "in"
                },
                new ParameterType
                {
                    ParameterTypeId = Guid.Parse("7e4f3023-43bb-41dc-9cf5-7f873a04c2fc"),
                    Name = "out"
                });
        }
    }
}