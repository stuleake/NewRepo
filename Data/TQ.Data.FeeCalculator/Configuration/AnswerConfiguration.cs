using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Answer Configuration for seed data.
    /// </summary>
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Answers", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new Answer
                {
                    AnswerId = Guid.Parse("a7aab1b5-5f43-4bb8-bcb1-d88ef3a576dd"),
                    ParameterName = "Is_Residential",
                    SessionType = "Qsr",
                    SessionId = GuidConstants.UserOneSessionGuid,
                    ParameterAnswer = bool.TrueString,
                    RowNo = 1
                },
                new Answer
                {
                    AnswerId = Guid.Parse("6bf54fcb-3ef5-4c85-b82f-97a3b6ccb95f"),
                    ParameterName = "Is_EnlargementImprovementAlteration",
                    SessionType = "Qsr",
                    SessionId = GuidConstants.UserOneSessionGuid,
                    ParameterAnswer = bool.TrueString,
                    RowNo = 2
                },
                new Answer
                {
                    AnswerId = Guid.Parse("7b4886ce-d3c4-45b7-87bb-eb9c8029a523"),
                    ParameterName = "Is_EnlargementImprovementAlteration_Single",
                    SessionType = "Qsr",
                    SessionId = GuidConstants.UserOneSessionGuid,
                    ParameterAnswer = bool.TrueString,
                    RowNo = 3
                },
                new Answer
                {
                    AnswerId = Guid.Parse("a2dfed53-5d34-49ab-9802-8274328a7f77"),
                    ParameterName = "Is_NewDwellings",
                    SessionType = "Qsr",
                    SessionId = GuidConstants.UserTwoSessionGuid,
                    ParameterAnswer = bool.TrueString,
                    RowNo = 4
                },
                new Answer
                {
                    AnswerId = Guid.Parse("4c9fa391-05fa-4cde-b79e-209261a617cf"),
                    ParameterName = "Num_NewDwellings",
                    SessionType = "Qsr",
                    SessionId = GuidConstants.UserTwoSessionGuid,
                    ParameterAnswer = "3",
                    RowNo = 5
                },
                new Answer
                {
                    AnswerId = Guid.Parse("0bc644a9-9045-41b7-af89-91ea58c3b6f3"),
                    ParameterName = "Is_NewDwellings",
                    SessionType = "Qsr",
                    SessionId = GuidConstants.UserThreeSessionGuid,
                    ParameterAnswer = bool.FalseString,
                    RowNo = 6
                },
                new Answer
                {
                    AnswerId = Guid.Parse("7a5ae97f-f0a8-462f-8ba6-86ce3b54ee62"),
                    ParameterName = "Num_NewDwellings",
                    SessionType = "Qsr",
                    SessionId = GuidConstants.UserThreeSessionGuid,
                    ParameterAnswer = "0",
                    RowNo = 7
                });
        }
    }
}