namespace Survey.xIntegrationTests.Clients
{
    public class SurveyClients
    {
        public string GetPutDeletePostSurvey;
        public string CreateOrGetAllSurvey;
        public SurveyClients(int companyId)
        {
            CreateOrGetAllSurvey = $"api/{companyId}/surveys/";
        }

        public SurveyClients(int companyId, int surveyId)
        {
            GetPutDeletePostSurvey = $"api/{companyId}/surveys/{surveyId}/";
        }
    }
}
