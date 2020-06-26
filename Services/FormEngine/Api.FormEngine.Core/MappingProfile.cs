using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Commands.Forms.Temp;
using Api.FormEngine.Core.ViewModels.QuestionSets;
using AutoMapper;
using TQ.Data.FormEngine.Schemas.Sessions;
using TQ.Data.FormEngine.StoredProcedureModel;

namespace Api.FormEngine.Core
{
    /// <summary>
    /// Mapping profile
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<SubmitQuestionSetResponsePP2, CreateApplication>().ReverseMap();
            CreateMap<GetQuestionSetResponse, ValidateQuestionSetResponse>().ReverseMap();

            CreateMap<TQ.Data.FormEngine.Schemas.Forms.QS, ViewModels.QuestionSets.QuestionSetDetails>();
            CreateMap<ViewModels.QuestionSets.QSdbDetail, ViewModels.QuestionSets.QuestionSetDetails>();
            CreateMap<ViewModels.QuestionSets.QSdbDetail, ViewModels.QuestionSets.Section>();
            CreateMap<ViewModels.QuestionSets.QSdbDetail, ViewModels.QuestionSets.Field>();

            CreateMap<ViewModels.QuestionSets.QSdbDetail, ViewModels.QuestionSets.AnswerGuide>();

            CreateMap<ViewModels.QuestionSets.AnswerGuide, ViewModels.QuestionSets.Option>()
                .ForMember(dest => dest.Key, o => o.MapFrom(source => source.AnswerGuideLabel))
                .ForMember(dest => dest.Value, o => o.MapFrom(source => source.AnswerGuideValue));

            CreateMap<ViewModels.QuestionSets.AnswerGuide, ViewModels.QuestionSets.FieldRule>()
                .ForMember(dest => dest.Name, o => o.MapFrom(source => source.AnswerType))
                .ForMember(dest => dest.Value, o => o.MapFrom(source => source.AnswerGuideValue))
                 .ForMember(dest => dest.Pattern, o => o.MapFrom(source => source.AnswerGuidePattern))
                .ForMember(dest => dest.Error, o => o.MapFrom(source => source.AnswerGuideError))
                .ForMember(dest => dest.Max, o => o.MapFrom(source => source.Max))
                .ForMember(dest => dest.Min, o => o.MapFrom(source => source.Min));
            CreateMap<QSCollection, ViewModels.UserApplication>()
                .ForMember(dest => dest.UserApplicationId, o => o.MapFrom(src => src.QSCollectionId))
                .ForMember(dest => dest.PlanningFormId, o => o.MapFrom(src => src.QSCollectionTypeId));

            CreateMap<ViewModels.SheetModels.QuestionSet, TQ.Data.FormEngine.Schemas.Forms.QS>()
                .ForMember(dest => dest.QSNo, o => o.MapFrom(src => src.QsNo))
                .ForMember(dest => dest.Label, o => o.MapFrom(src => src.QSLabel))
                .ForMember(dest => dest.Helptext, o => o.MapFrom(src => src.QSHelptext))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.QSDesc));
            CreateMap<ViewModels.SheetModels.Section, TQ.Data.FormEngine.Schemas.Forms.Section>()
                .ForMember(dest => dest.Label, o => o.MapFrom(src => src.SectionLabel))
                .ForMember(dest => dest.Helptext, o => o.MapFrom(src => src.Sectionhelptext))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.SectionDesc))
                .ForMember(dest => dest.SectionType, opt => opt.Ignore());
            CreateMap<ViewModels.SheetModels.Field, TQ.Data.FormEngine.Schemas.Forms.Field>()
                 .ForMember(dest => dest.Label, o => o.MapFrom(src => src.FieldLabel))
                 .ForMember(dest => dest.Helptext, o => o.MapFrom(src => src.Fieldhelptext))
                 .ForMember(dest => dest.Description, o => o.MapFrom(src => src.FieldDesc))
                 .ForMember(dest => dest.FieldType, o => o.Ignore());

            CreateMap<TQ.Data.FormEngine.Schemas.Forms.QSCollectionType, ViewModels.ApplicationTypeModel>();

            CreateMap<ViewModels.QuestionSets.FieldConstraintDetail, ViewModels.QuestionSets.Condition>()
               .ForMember(dest => dest.On, o => o.MapFrom(src => src.DependantAnswerFieldId))
               .ForMember(dest => dest.Value, o => o.MapFrom(src => src.AnswerValue));

            CreateMap<QuestionSetDetailSpModel, QSdbDetail>()
                .ForMember(dest => dest.FieldVersion, o => o.MapFrom(src => src.FieldVersion.ToString()))
                .ForMember(dest => dest.QSVersion, o => o.MapFrom(src => src.QSVersion.ToString()));

            CreateMap<FieldConstraintDetailSpModel, FieldConstraintDetail>();
        }
    }
}