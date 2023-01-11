namespace Survey.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionID { get; set; }

        [ForeignKey("AnwserBlockID")]
        public int AnwserBlockID { get; set; }

        [Required]
        public string Code { get; set; }

        [MaxLength(100)]
        public string QuestionText { get; set; }

        [MaxLength(255)]
        public string QuestionType { get; set; }

        [ForeignKey("SurveyID")]
        public int? SurveyID { get; set; }

        public virtual AnwserBlock? AnwserBlock { get; set; }

        public virtual Surveys? Survey { get; set; }
    }
}
