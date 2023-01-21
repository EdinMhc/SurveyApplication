namespace Survey.API.Profiles
{

    public class SurveyReportConfiguration : Profile
    {
        public SurveyReportConfiguration()
        {
            // Source --> Destination
            CreateMap<Survey.Infrastructure.Entities.SurveyReport, Survey.API.DTOs.SurveyReportDtos.SurveyReportBasicInfoDto>();
            CreateMap<SurveyReportBasicInfoDto, Survey.Infrastructure.Entities.SurveyReport>();


            CreateMap<Survey.Infrastructure.Entities.SurveyReport, Survey.API.DTOs.SurveyReportDtos.SurveyReportForCreationDto>();
            CreateMap<SurveyReportForCreationDto, Survey.Infrastructure.Entities.SurveyReport>();

            CreateMap<Survey.Infrastructure.Entities.SurveyReport, Survey.API.DTOs.SurveyReportDtos.SurveyReportUpdateDtos>();
            CreateMap<SurveyReportUpdateDtos, Survey.Infrastructure.Entities.SurveyReport>();
        }
    }
}
