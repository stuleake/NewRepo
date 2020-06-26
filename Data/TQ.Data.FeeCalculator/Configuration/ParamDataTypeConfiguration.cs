using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Parameter Data Type Configuration for seed data.
    /// </summary>
    public class ParamDataTypeConfiguration : IEntityTypeConfiguration<ParamDataType>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<ParamDataType> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("ParamDataTypes", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new ParamDataType
                {
                    ParamDataTypeId = GuidConstants.BoolParameterDataTypeGuid,
                    ParamDataTypeName = "bool"
                },
                new ParamDataType
                {
                    ParamDataTypeId = GuidConstants.NumberParameterDataTypeGuid,
                    ParamDataTypeName = "number"
                },
                new ParamDataType
                {
                    ParamDataTypeId = GuidConstants.IntParameterTDataypeGuid,
                    ParamDataTypeName = "int"
                },
                new ParamDataType
                {
                    ParamDataTypeId = GuidConstants.StringParameterDataTypeGuid,
                    ParamDataTypeName = "string"
                });
        }
    }
}