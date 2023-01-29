namespace Survey.API.DTOs.Company
{

    public class CompanyDto
    {
        public string UserID { get; set; }

        public int CompanyId { get; set; }

        public string? CompanyName { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual IEnumerable<SurveyDto>? Surveys { get; set; }
    }
}
