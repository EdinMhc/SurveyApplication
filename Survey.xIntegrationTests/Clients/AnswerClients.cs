namespace Survey.xIntegrationTests.Clients
{
    public class AnswerClients
    {
        public string PostGetAll;
        public string UpdateGetDelete;

        public AnswerClients(int companyId, int answerBlockId)
        {
            PostGetAll = $"api/{companyId}/{answerBlockId}/answer";
        }

        public AnswerClients(int companyId, int answerBlockId, int answerId)
        {
            UpdateGetDelete = $"api/{companyId}/{answerBlockId}/answer/{answerId}";
        }
    }
}
