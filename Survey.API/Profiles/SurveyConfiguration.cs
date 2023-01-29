namespace Survey.API.Profiles
{

    public class SurveyConfiguration : Profile
    {
        public SurveyConfiguration()
        {
            // Source --> Destination
            CreateMap<Survey.Infrastructure.Entities.Surveys, SurveyDto>();
            CreateMap<SurveyDto, Survey.Infrastructure.Entities.Surveys>();

            CreateMap<Survey.Infrastructure.Entities.Surveys, SurveyCreationDto>();
            CreateMap<SurveyCreationDto, Survey.Infrastructure.Entities.Surveys>();

            CreateMap<Survey.Infrastructure.Entities.Surveys, SurveyUpdateDto>();
            CreateMap<SurveyUpdateDto, Survey.Infrastructure.Entities.Surveys>();
        }
    }
}
