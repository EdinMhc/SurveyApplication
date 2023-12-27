namespace Survey.xIntegrationTests.Clients
{
    public class QuestionClients
    {
        public string GetAllOrPost;
        public string GetDeletePut;

        public QuestionClients(int companyId, int surveyId, int questionId = 0)
        {
            GetAllOrPost = $"api/{companyId}/questions/{surveyId}";
            GetDeletePut = $"api/{companyId}/questions/{surveyId}/{questionId}";
        }
    }
}
