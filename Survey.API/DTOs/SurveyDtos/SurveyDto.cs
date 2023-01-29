namespace Survey.API.DTOs.SurveyDtos
{
    public class SurveyDto
    {
        public int SurveyID { get; set; }

        public int CompanyID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

        public string SurveyName { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<SurveyReportBasicInfoDto>? SurveyReport { get; set; }
    }
}
