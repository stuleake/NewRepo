using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FormEngine.Schemas.Forms;

namespace TQ.Data.FormEngine.Configuration
{
    /// <summary>
    /// AnswerType Configuration for Seed data.
    /// </summary>
    public class AnswerTypeConfiguration : IEntityTypeConfiguration<AnswerType>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<AnswerType> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("AnswerTypes", FormEngineSchemas.Forms);

            // TODO: convert to enum
            builder.HasData(
                new AnswerType
                {
                    AnswerTypeId = 1,
                    AnswerTypes = "Range"
                },
                new AnswerType
                {
                    AnswerTypeId = 2,
                    AnswerTypes = "Length"
                },
                new AnswerType
                {
                    AnswerTypeId = 3,
                    AnswerTypes = "regex"
                },
                new AnswerType
                {
                    AnswerTypeId = 4,
                    AnswerTypes = "RegexBE"
                },
                new AnswerType
                {
                    AnswerTypeId = 5,
                    AnswerTypes = "Multiple"
                },
                new AnswerType
                {
                    AnswerTypeId = 6,
                    AnswerTypes = "Value"
                },
                new AnswerType
                {
                    AnswerTypeId = 7,
                    AnswerTypes = "API"
                },
                new AnswerType
                {
                    AnswerTypeId = 8,
                    AnswerTypes = "Date"
                },
                new AnswerType
                {
                    AnswerTypeId = 9,
                    AnswerTypes = "copyFrom"
                });
        }
    }
}