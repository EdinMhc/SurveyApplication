namespace Survey.Infrastructure.Repositories
{
    using Survey.Infrastructure.DapperRepository;
    using Survey.Infrastructure.Entities;

    public interface IUnitOfWork
    {
        IRepository<Company> CompanyRepository { get; }

        IRepository<Surveys> SurveysRepository { get; }

        IRepository<AnwserBlock> AnwserBlockRepository { get; }

        IRepository<Anwser> AnwserRepository { get; }

        IRepository<SurveyReport> SurveyReportRepository { get; }

        IRepository<Question> QuestionRepository { get; }

        IRepository<User> UserRepository { get; }

        IRepository<SurveyReportData> SurveyReportDataRepository { get; }

        IGenericRepository<Company> companyGenericRepository { get; }

        Task SaveChangesAsync();
    }
}
