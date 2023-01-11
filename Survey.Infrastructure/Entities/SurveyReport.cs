namespace Survey.Infrastructure.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SurveyReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SurveyReportID { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreateDate { get; set; }

        [ForeignKey("SurveyID")]
        public int SurveyID { get; set; }

        public virtual Surveys Survey { get; set; }

        public virtual ICollection<SurveyReportData>? SurveyReportData { get; set; }
    }
}
