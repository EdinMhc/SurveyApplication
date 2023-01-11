namespace Survey.API.DTOs.QuestionDtos
{
    public class QuestionBasicInfoDto
    {
        public int QuestionID { get; set; }

        public int AnwserBlockID { get; set; }

        public string Code { get; set; }

        public string QuestionText { get; set; }

        public string QuestionType { get; set; }

        public int SurveyID { get; set; }
    }
}
