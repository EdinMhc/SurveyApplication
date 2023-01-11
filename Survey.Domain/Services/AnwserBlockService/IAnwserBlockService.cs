namespace Survey.Domain.Services.AnwserBlockService
{
    using Survey.Infrastructure.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAnwserBlockService
    {
        public IEnumerable<AnwserBlock> GetAll(int companyId, int surveyId, string? role, string userId);

        AnwserBlock GetById(int anwserBlockId, int companyId, int surveyId, string role, string userId);

        public Task<AnwserBlock> CreateAsync(AnwserBlock anwserBlockId, int companyId, int surveyId, string? role, string userId);

        public Task<AnwserBlock> UpdateAsync(AnwserBlock anwserBlock, int companyId, int surveyId, int answerBlockId, string? role, string userId);

        public Task<bool> DeleteAsync(int companyId, int surveyId, int questionId, string? role, string userId);
    }
}
