namespace Survey.xIntegrationTests.Clients
{
    public class AnswerBlockClients
    {
        public string GetAllOrPostAnswerBlock;
        public string GetPutDeleteAnswerBlock;

        public AnswerBlockClients(int companyId, int surveyId, int answerBlockId = 0)
        {
            GetAllOrPostAnswerBlock = $"api/{companyId}/answerblock/{surveyId}";
            GetPutDeleteAnswerBlock = $"api/{companyId}/answerblock/{surveyId}/{answerBlockId}";
        }
    }
}
