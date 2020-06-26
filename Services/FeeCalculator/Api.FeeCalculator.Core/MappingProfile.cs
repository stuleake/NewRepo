using AutoMapper;
using System;
using System.Globalization;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace Api.FeeCalculator.Core
{
    /// <summary>
    /// Mapping profile class to configure auto mapper
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<ViewModels.RuleDefModel, RuleDef>()
                .ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => src.RuleId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate ?? DateTime.ParseExact("01/01/2018", "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Category, opt => opt.Ignore());
            CreateMap<Step, ViewModels.CalculationStepModel>();
            CreateMap<Step, ViewModels.FinalOutputModel>()
                .ForMember(dest => dest.ParameterName, opt => opt.MapFrom(src => src.OutputParamName))
                .ForMember(dest => dest.DataType, opt => opt.MapFrom(src => src.OutputDataType))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Output));
        }
    }
}