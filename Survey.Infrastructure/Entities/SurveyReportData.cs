namespace Survey.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SurveyReportData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RespondentID { get; set; }

        [ForeignKey("SurveyReportID")]
        public int SurveyReportID { get; set; }

        public int QuestionID { get; set; }

        public int AnswerID { get; set; }

        public DateTime CreatedDate { get; set; }

        public SurveyReport? SurveyReport { get; set; }
    }
}
