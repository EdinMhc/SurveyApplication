namespace Survey.API.Profiles
{
    using AutoMapper;
    using Survey.API.DTOs.SurveyDtos;

    public class SurveyConfiguration : Profile
    {
        public SurveyConfiguration()
        {
            // Source --> Destination
            this.CreateMap<Survey.Infrastructure.Entities.Surveys, SurveyBasicInfoDto>();
            this.CreateMap<SurveyBasicInfoDto, Survey.Infrastructure.Entities.Surveys>();

            this.CreateMap<Survey.Infrastructure.Entities.Surveys, SurveyForCreationDto>();
            this.CreateMap<SurveyForCreationDto, Survey.Infrastructure.Entities.Surveys>();

            this.CreateMap<Survey.Infrastructure.Entities.Surveys, SurveyUpdateDto>();
            this.CreateMap<SurveyUpdateDto, Survey.Infrastructure.Entities.Surveys>();
        }
    }
}
