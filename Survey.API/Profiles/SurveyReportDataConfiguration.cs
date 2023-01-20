namespace Survey.API.Profiles
{
    using AutoMapper;
    using Survey.API.DTOs.SurveyReportData;
    using Survey.Infrastructure.Entities;

    public class SurveyReportDataConfiguration : Profile
    {
        public SurveyReportDataConfiguration()
        {
            CreateMap<SurveyReportData, SurveyReportDataBasicInfoDto>();
            CreateMap<Survey.API.DTOs.SurveyReportData.SurveyReportDataBasicInfoDto, Survey.Infrastructure.Entities.SurveyReportData>();

            CreateMap<SurveyReportData, SurveyReportDataForCreationDto>();
            CreateMap<Survey.API.DTOs.SurveyReportData.SurveyReportDataForCreationDto, Survey.Infrastructure.Entities.SurveyReportData>();

            CreateMap<SurveyReportData, SurveyReportDataUpdateDto>();
            CreateMap<Survey.API.DTOs.SurveyReportData.SurveyReportDataUpdateDto, Survey.Infrastructure.Entities.SurveyReportData>();
        }
    }
}
