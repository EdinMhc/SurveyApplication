namespace Survey.API.DTOs.SurveyDtos
{
    using Survey.API.DTOs.SurveyReportDtos;

    public class SurveyBasicInfoDto
    {
        public int SurveyID { get; set; }

        public int CompanyID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

        public string SurveyName { get; set; }

        // Eager loading was solved with displaying the data inside it's dtos
        public IEnumerable<SurveyReportBasicInfoDto>? SurveyReport { get; set; }
    }
}
