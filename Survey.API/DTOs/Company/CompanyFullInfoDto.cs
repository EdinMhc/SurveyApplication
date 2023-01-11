namespace Survey.API.DTOs.Company
{
    public class CompanyFullInfoDto
    {
        public int? CompanyID { get; set; }

        public string? CreatedBy { get; set; }

        public string? CompanyName { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
