using Api.PP2.Core.Commands.Forms;
using AutoMapper;
using TQ.Data.PlanningPortal.Schemas.Dbo;

namespace Api.PP2.Core
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
            CreateMap<SubmitQuestionSetResponse, CreateApplication>().ReverseMap();
            CreateMap<SubmitQuestionSetResponse, QuestionSetResponse>().ReverseMap();
            CreateMap<CreateApplication, UserApplication>().ReverseMap();
            CreateMap<UserApplication, ViewModels.UserApplication>();
        }
    }
}