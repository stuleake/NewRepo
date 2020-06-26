using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Globalization;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Question Set Response (answers) Configuration for seed data.
    /// </summary>
    public class QsrConfiguration : IEntityTypeConfiguration<Qsr>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Qsr> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable("Qsr", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new Qsr
                {
                    QsrId = Guid.Parse("f59d9eea-e696-4e75-adf5-4d2254187e58"),
                    QSCollectionId = GuidConstants.QSCollectionGuid,
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = GuidConstants.TestApplicantGuid
                },
                new Qsr
                {
                    QsrId = Guid.Parse("7fd1318f-0694-4abc-826f-3fef22f0bc3c"),
                    QSCollectionId = GuidConstants.QSCollectionGuid,
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = GuidConstants.TestApplicantGuid
                });
        }
    }
}