namespace Survey.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyID { get; set; }

        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }

        [ForeignKey("UserID")]
        public string UserID { get; set; }

        public User User { get; set; } = new();

        public virtual ICollection<AnwserBlock>? AnswerBlock { get; set; }

        public ICollection<Surveys>? Surveys { get; set; }
    }
}
