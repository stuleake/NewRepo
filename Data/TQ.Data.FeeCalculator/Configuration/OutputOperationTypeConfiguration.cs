using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Output Operation Type Configuration for seed data.
    /// </summary>
    public class OutputOperationTypeConfiguration : IEntityTypeConfiguration<OutputOperationType>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<OutputOperationType> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("OutputOperationTypes", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new OutputOperationType
                {
                    OutputOperationId = Guid.Parse("10380bc9-98dd-49c2-b9ca-b0d316f9707a"),
                    Name = "add"
                },
                new OutputOperationType
                {
                    OutputOperationId = Guid.Parse("ccfcb7fc-4705-4cf6-904e-cd2bc382ab9e"),
                    Name = "overwrite"
                });
        }
    }
}