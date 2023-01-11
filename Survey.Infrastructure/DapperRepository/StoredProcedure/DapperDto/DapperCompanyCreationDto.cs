namespace Survey.Infrastructure.DapperRepository.StoredProcedure.DapperDto
{
    using System.ComponentModel.DataAnnotations;

    public class DapperCompanyCreationDto
    {
        [Required]
        public int CompanyID { get; set; }

        public string? CompanyName { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
