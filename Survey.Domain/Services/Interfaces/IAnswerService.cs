namespace Survey.Domain.Services.Interfaces
{
    using Survey.Infrastructure.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAnswerService
    {
        public Task<Anwser> CreateAsync(Anwser answer, int companyId, int anwserBlockId, string? role, string userId);

        public Anwser GetById(int companyId, int anwserBlockId, int anwserId, string? role, string userId);

        public Task<Anwser> UpdateAsync(Anwser answer, int companyId, int anwserBlockId, int answerId, string? role, string userId);

        public Task<bool> DeleteAsync(int companyId, int answerBlockId, int answerId, string? role, string userId);

        public IEnumerable<Anwser> GetAll(int companyId, int anwserBlockId, string? role, string userId);
    }
}
