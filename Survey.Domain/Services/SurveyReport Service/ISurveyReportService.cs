namespace Survey.Domain.Services.SurveyReport_Service
{
    using Survey.Infrastructure.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISurveyReportService
    {
        public Task<SurveyReport> CreateAsync(SurveyReport surveyReport, int companyId, int surveyId, string? role, string userId);

        SurveyReport GetById(int companyId, int surveyId, int surveyReportId, string? role, string userId);

        public Task<SurveyReport> UpdateAsync(SurveyReport surveyReport, int companyId, int surveyId, int surveyReportId, string? role, string userId);

        public Task<bool> DeleteAsync(int companyId, int surveyId, int surveyReportId, string? role, string userId);

        public IEnumerable<SurveyReport> GetAll(int companyId, int surveyId, string? userId, string? role);
    }
}
