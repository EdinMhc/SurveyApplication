namespace Survey.xIntegrationTests.Fixtures
{
    public class QuestionFixture : FixtureImp
    {
        public string AnswerBlockNotValid = "AnwserBlock does not exist";
        public string CodeNotValid = "Question Code is shorter or longer than required";
        public string QuestionTextNotValid = "Question Text is shorter or longer than required";
        public string QuestionTypeNotValid = "Question Type is shorter or longer than required";

        public QuestionFixture(WebApplicationFactory<Program> factory) : base(factory)
        {

        }
    }
}
