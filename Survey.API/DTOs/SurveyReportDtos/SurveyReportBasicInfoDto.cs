namespace Survey.API.DTOs.SurveyReportDtos
{
    public class SurveyReportBasicInfoDto
    {
        public int SurveyReportID { get; set; }

        public bool? IsCompleted { get; set; }

        public DateTime CreateDate { get; set; }

        public IEnumerable<SurveyReportDataBasicInfoDto> SurveyReportData { get; set; }
    }
}
