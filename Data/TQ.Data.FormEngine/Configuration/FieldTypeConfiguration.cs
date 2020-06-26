using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FormEngine.Schemas.Forms;

namespace TQ.Data.FormEngine.Configuration
{
    /// <summary>
    /// FieldType Configuration for Seed data.
    /// </summary>
    public class FieldTypeConfiguration : IEntityTypeConfiguration<FieldType>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<FieldType> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("FieldTypes", FormEngineSchemas.Forms);

            // TODO: convert to enum
            builder.HasData(
                new FieldType
                {
                    FieldTypeId = 1,
                    FieldTypes = "NUMBER"
                },
                new FieldType
                {
                    FieldTypeId = 2,
                    FieldTypes = "DROPDOWN"
                },
                new FieldType
                {
                    FieldTypeId = 3,
                    FieldTypes = "DATE"
                },
                new FieldType
                {
                    FieldTypeId = 4,
                    FieldTypes = "DROPDOWN"
                },
                new FieldType
                {
                    FieldTypeId = 5,
                    FieldTypes = "BUTTON"
                },
                new FieldType
                {
                    FieldTypeId = 6,
                    FieldTypes = "ActionInput"
                },
                new FieldType
                {
                    FieldTypeId = 7,
                    FieldTypes = "ActionAddress"
                },
                new FieldType
                {
                    FieldTypeId = 8,
                    FieldTypes = "ActionTable"
                },
                new FieldType
                {
                    FieldTypeId = 9,
                    FieldTypes = "Aggregation"
                },
                new FieldType
                {
                    FieldTypeId = 10,
                    FieldTypes = "TEXT"
                },
                new FieldType
                {
                    FieldTypeId = 11,
                    FieldTypes = "NUMBERSELECTOR"
                },
                new FieldType
                {
                    FieldTypeId = 12,
                    FieldTypes = "Notification"
                },
                new FieldType
                {
                    FieldTypeId = 13,
                    FieldTypes = "CHECKBOX"
                });
        }
    }
}