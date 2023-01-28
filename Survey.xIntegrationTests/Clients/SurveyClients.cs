namespace Survey.xIntegrationTests.Clients
{
    public class SurveyClients
    {
        public string GetPutDeletePostSurvey;
        public string GetAllOrCreateSurvey;
        public SurveyClients(int companyId)
        {
            GetAllOrCreateSurvey = $"api/{companyId}/surveys/";
        }

        public SurveyClients(int companyId, int surveyId)
        {
            GetPutDeletePostSurvey = $"api/{companyId}/surveys/{surveyId}/";
        }
    }
}
