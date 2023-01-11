namespace Survey.Domain.Services.SurveyReportDataService
{
    using Survey.Infrastructure.Entities;

    public interface ISurveyReportDataService
    {
        public Task<SurveyReportData> CreateAsync(SurveyReportData surveyReport, int companyId, int surveyId, int surveyReportId, string? role, string userId);

        SurveyReportData GetById(int companyId, int surveyId, int surveyReportId, int respondentId, string? role, string userId);

        public Task<SurveyReportData> UpdateAsync(SurveyReportData surveyReport, int companyId, int surveyId, int surveyReportId, int respondentId, string? role, string userId);

        public Task<bool> DeleteAsync(int companyId, int surveyId, int surveyReportId, int respondentId, string? role, string userId);

        public IEnumerable<SurveyReportData> GetAll(int companyId, int surveyId, int surveyReportId, string? role, string userId);
    }
}
