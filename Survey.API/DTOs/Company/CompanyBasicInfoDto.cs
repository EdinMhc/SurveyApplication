namespace Survey.API.DTOs.Company
{

    public class CompanyBasicInfoDto
    {
        public string UserID { get; set; }

        public int CompanyId { get; set; }

        public string? CompanyName { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual IEnumerable<SurveyBasicInfoDto>? Surveys { get; set; }
    }
}
