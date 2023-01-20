namespace Survey.Domain.Services.Interfaces
{
    using Survey.Infrastructure.Entities;

    public interface IQuestionService
    {
        public IEnumerable<Question> GetAll(int companyId, int surveyId, string? role, string userId);

        Question GetById(int companyId, int surveyId, int questionId, string? role, string userId);

        public Task<Question> CreateAsync(Question question, int companyId, int surveyId, string? role, string userId);

        public Task<Question> UpdateAsync(Question question, int companyId, int surveyId, int questionId, string? role, string userId);

        public Task<bool> DeleteAsync(int companyId, int surveyId, int questionId, string? role, string userId);
    }
}
