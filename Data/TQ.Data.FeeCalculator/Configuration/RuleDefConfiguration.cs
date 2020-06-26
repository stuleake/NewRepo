using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Globalization;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Rule Definition Configuration for seed data.
    /// </summary>
    public class RuleDefConfiguration : IEntityTypeConfiguration<RuleDef>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<RuleDef> builder)
        {
            const string pp2Product = "PP2";
            const string englandTenant = "England";

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("RuleDefs", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new RuleDef
                {
                    RuleDefId = GuidConstants.ResidentialDwellingAlterationRuleGuid,
                    DefPackageId = GuidConstants.DraftDefPackageGuid,
                    ReferenceId = 1,
                    RuleName = "Residential Dwelling Alteration",
                    RuleDefinition = "if (Is_Residential && Is_EnlargementImprovementAlteration)" +
                                     "{if (Is_EnlargementImprovementAlteration_Single) {OtherMixedDevelopmentFee = EnlargementImprovementAlterationSingleCharge;" +
                                     "Desc = 'Fee for enlargement, improvement or alteration of a flat: £' + EnlargementImprovementAlterationSingleCharge;}" +
                                     "else if (Is_EnlargementImprovementAlteration_Multiple){OtherMixedDevelopmentFee = EnlargementImprovementAlterationMultipleCharge;" +
                                     "Desc = 'Fee for enlargement, improvement or alteration of two or more dwellings: £' + EnlargementImprovementAlterationMultipleCharge}" +
                                     "output = {'description': Desc,'outputparametervalue': OtherMixedDevelopmentFee};return JSON.stringify(output)}",
                    CategoryId = GuidConstants.FeeRulesCategoryGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(7),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = GuidConstants.TestUserGuid
                },
                new RuleDef
                {
                    RuleDefId = GuidConstants.CreateNewDwellingRuleGuid,
                    DefPackageId = GuidConstants.DraftDefPackageGuid,
                    ReferenceId = 2,
                    RuleName = "Create New Dwellings",
                    RuleDefinition = "if (Is_Residential && Is_NewDwellings){if (!isNaN(Num_NewDwellings) && Num_NewDwellings <= 50) " +
                                     "{ResidentialFee = parseInt(Num_NewDwellings) * NewDwellingCharge; Desc = '£' + NewDwellingCharge + ' per new dwelling';}" +
                                     "else if (!isNaN(Num_NewDwellings) && Num_NewDwellings > 50) {ResidentialFee = Math.min(NewDwellingFlatCharge + (parseInt(Num_NewDwellings) - 50) * " +
                                     "AdditionalNewDwellingCharge, MaxNewDwellingCharge);Desc = 'Fee = ' + NewDwellingFlatCharge + ' + £' + AdditionalNewDwellingCharge + ' " +
                                     "for each additional dwellinghouse in excess of 50. Maximum total = £' + MaxNewDwellingCharge;}" +
                                     "output = {'description': Desc,'outputparametervalue': ResidentialFee};return JSON.stringify(output)}",
                    CategoryId = GuidConstants.FeeRulesCategoryGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(9),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = GuidConstants.TestUserGuid
                });
        }
    }
}