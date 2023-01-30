namespace Survey.xIntegrationTests.Clients
{
    public class QuestionClients
    {
        public string GetAllAndPost;
        public string GetDeletePut;

        public QuestionClients(int companyId, int surveyId, int questionId = 0)
        {
            GetAllAndPost = $"api/{companyId}/questions/{surveyId}";
            GetDeletePut = $"api/{companyId}/questions/{surveyId}/{questionId}";
        }
    }
}
