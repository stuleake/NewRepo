using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator.Configuration
{
    /// <summary>
    /// Parameter Configuration for seed data.
    /// </summary>
    public class ParameterConfiguration : IEntityTypeConfiguration<Parameter>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Parameter> builder)
        {
            const string pp2Product = "PP2";
            const string englandTenant = "England";

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Parameters", FeeCalculatorSchemas.Dbo);
            builder.HasData(
                new Parameter
                {
                    ParameterId = GuidConstants.IsResidentialParameterGuid,
                    Name = "Is_Residential",
                    ParamDataTypeId = GuidConstants.BoolParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = bool.FalseString
                },
                new Parameter
                {
                    ParameterId = GuidConstants.IsEnglargementImprovementAlterationParameterGuid,
                    Name = "Is_EnlargementImprovementAlteration",
                    ParamDataTypeId = GuidConstants.BoolParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = bool.FalseString
                },
                new Parameter
                {
                    ParameterId = GuidConstants.IsEnlargementImprovementAlterationSingleParameterGuid,
                    Name = "Is_EnlargementImprovementAlteration_Single",
                    ParamDataTypeId = GuidConstants.BoolParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = bool.FalseString
                },
                new Parameter
                {
                    ParameterId = GuidConstants.IsEnlargementImprovementAlterationMultipleParameterGuid,
                    Name = "Is_EnlargementImprovementAlteration_Multiple",
                    ParamDataTypeId = GuidConstants.BoolParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = bool.FalseString
                },
                new Parameter
                {
                    ParameterId = GuidConstants.EnlargementImprovementAlterationSingleChargeParameterGuid,
                    Name = "EnlargementImprovementAlterationSingleCharge",
                    ParamDataTypeId = GuidConstants.NumberParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = "206"
                },
                new Parameter
                {
                    ParameterId = GuidConstants.EnlargementImprovementAlterationMultipleChargeParameterGuid,
                    Name = "EnlargementImprovementAlterationMultipleCharge",
                    ParamDataTypeId = GuidConstants.NumberParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = "407"
                },
                new Parameter
                {
                    ParameterId = GuidConstants.OtherMixedDevelopmentFeeParameterGuid,
                    Name = "OtherMixedDevelopmentFee",
                    ParamDataTypeId = GuidConstants.NumberParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = "0"
                },
                new Parameter
                {
                    ParameterId = GuidConstants.IsNewDwellingsParameterGuid,
                    Name = "Is_NewDwellings",
                    ParamDataTypeId = GuidConstants.BoolParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = bool.FalseString
                },
                new Parameter
                {
                    ParameterId = GuidConstants.NumNewDwellingsParameterGuid,
                    Name = "Num_NewDwellings",
                    ParamDataTypeId = GuidConstants.IntParameterTDataypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = "0"
                },
                new Parameter
                {
                    ParameterId = GuidConstants.NewDwellingChargeParameterGuid,
                    Name = "NewDwellingCharge",
                    ParamDataTypeId = GuidConstants.NumberParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = "462"
                },
                new Parameter
                {
                    ParameterId = GuidConstants.NewDwellingFlatChargeParameterGuid,
                    Name = "NewDwellingFlatCharge",
                    ParamDataTypeId = GuidConstants.NumberParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = "22859"
                },
                new Parameter
                {
                    ParameterId = GuidConstants.AdditionalNewDwellingChargeParameterGuid,
                    Name = "AdditionalNewDwellingCharge",
                    ParamDataTypeId = GuidConstants.NumberParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = "22859"
                },
                new Parameter
                {
                    ParameterId = GuidConstants.MaxNewDwellingChargeParameterGuid,
                    Name = "MaxNewDwellingCharge",
                    ParamDataTypeId = GuidConstants.NumberParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = "300000"
                },
                new Parameter
                {
                    ParameterId = GuidConstants.ResidentialFeeParameterGuid,
                    Name = "ResidentialFee",
                    ParamDataTypeId = GuidConstants.NumberParameterDataTypeGuid,
                    Tenant = englandTenant,
                    Product = pp2Product,
                    MasterValue = "0"
                });
        }
    }
}