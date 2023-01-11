namespace Survey.API.DTOs.SurveyReportData
{
    public class SurveyReportDataBasicInfoDto
    {
        public int RespondentID { get; set; }

        public int SurveyReportID { get; set; }

        public int QuestionID { get; set; }

        public int AnswerID { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
