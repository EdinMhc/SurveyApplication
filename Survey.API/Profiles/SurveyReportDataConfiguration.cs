namespace Survey.API.Profiles
{
    using AutoMapper;
    using Survey.API.DTOs.SurveyReportData;
    using Survey.Infrastructure.Entities;

    public class SurveyReportDataConfiguration : Profile
    {
        public SurveyReportDataConfiguration()
        {
            this.CreateMap<SurveyReportData, SurveyReportDataBasicInfoDto>();
            this.CreateMap<Survey.API.DTOs.SurveyReportData.SurveyReportDataBasicInfoDto, Survey.Infrastructure.Entities.SurveyReportData>();

            this.CreateMap<SurveyReportData, SurveyReportDataForCreationDto>();
            this.CreateMap<Survey.API.DTOs.SurveyReportData.SurveyReportDataForCreationDto, Survey.Infrastructure.Entities.SurveyReportData>();

            this.CreateMap<SurveyReportData, SurveyReportDataUpdateDto>();
            this.CreateMap<Survey.API.DTOs.SurveyReportData.SurveyReportDataUpdateDto, Survey.Infrastructure.Entities.SurveyReportData>();
        }
    }
}
