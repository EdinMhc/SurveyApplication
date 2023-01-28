namespace Survey.API.DTOs.Company
{
    public class CompanyEditDto
    {
        [Required]
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}
