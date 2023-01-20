namespace Survey.Domain.Services.Interfaces
{
    using Survey.Infrastructure.Entities;

    public interface ISurveyService
    {
        public Task<Surveys> CreateAsync(Surveys survey, int companyId, string? role, string userId);

        Surveys GetById(int surveyId, int companyId, string? role, string userId);

        public Task<Surveys> UpdateAsync(Surveys survey, int surveyId, int companyId, string? role, string userId);

        public Task<bool> DeleteAsync(int surveyId, int companyId, string? role, string userId);

        public IEnumerable<Surveys> GetAll(int companyId, string? role, string? userId);
    }
}
