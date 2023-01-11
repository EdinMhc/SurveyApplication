namespace Survey.API.Profiles
{
    using AutoMapper;
    using Survey.API.DTOs.SurveyReportDtos;

    public class SurveyReportConfiguration : Profile
    {
        public SurveyReportConfiguration()
        {
            // Source --> Destination
            this.CreateMap<Survey.Infrastructure.Entities.SurveyReport, Survey.API.DTOs.SurveyReportDtos.SurveyReportBasicInfoDto>();
            this.CreateMap<SurveyReportBasicInfoDto, Survey.Infrastructure.Entities.SurveyReport>();


            this.CreateMap<Survey.Infrastructure.Entities.SurveyReport, Survey.API.DTOs.SurveyReportDtos.SurveyReportForCreationDto>();
            this.CreateMap<SurveyReportForCreationDto, Survey.Infrastructure.Entities.SurveyReport>();

            this.CreateMap<Survey.Infrastructure.Entities.SurveyReport, Survey.API.DTOs.SurveyReportDtos.SurveyReportUpdateDtos>();
            this.CreateMap<SurveyReportUpdateDtos, Survey.Infrastructure.Entities.SurveyReport>();
        }
    }
}
