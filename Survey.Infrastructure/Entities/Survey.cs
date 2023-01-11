namespace Survey.Infrastructure.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Surveys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SurveyID { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(255)]
        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        [MaxLength(255)]
        public string SurveyName { get; set; }

        public int? CompanyID { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<SurveyReport>? SurveyReport { get; set; }

        public virtual ICollection<Question>? Questions { get; set; }
    }
}
