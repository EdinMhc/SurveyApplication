namespace Survey.API.Profiles
{
    using AutoMapper;
    using Survey.API.DTOs.SurveyDtos;

    public class SurveyConfiguration : Profile
    {
        public SurveyConfiguration()
        {
            // Source --> Destination
            CreateMap<Survey.Infrastructure.Entities.Surveys, SurveyBasicInfoDto>();
            CreateMap<SurveyBasicInfoDto, Survey.Infrastructure.Entities.Surveys>();

            CreateMap<Survey.Infrastructure.Entities.Surveys, SurveyForCreationDto>();
            CreateMap<SurveyForCreationDto, Survey.Infrastructure.Entities.Surveys>();

            CreateMap<Survey.Infrastructure.Entities.Surveys, SurveyUpdateDto>();
            CreateMap<SurveyUpdateDto, Survey.Infrastructure.Entities.Surveys>();
        }
    }
}
