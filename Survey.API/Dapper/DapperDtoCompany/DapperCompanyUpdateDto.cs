namespace Survey.API.Dapper.DapperDtoCompany
{
    public class DapperCompanyUpdateDto
    {
        public int UserId { get; set; }

        public int CompanyID { get; set; }

        public string? CompanyName { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
