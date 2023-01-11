namespace Survey.Infrastructure.Repositories
{
    using Microsoft.Extensions.Configuration;
    using Survey.Infrastructure.ContextClass1;
    using Survey.Infrastructure.DapperRepository;
    using Survey.Infrastructure.Entities;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContextClass context;
        private IRepository<Company> companyRepository;
        private IRepository<Surveys> surveyRepository;
        private IRepository<Question> questionRepository;
        private IRepository<SurveyReport> surveyReportRepository;
        private IRepository<Anwser> anwserRepository;
        private IRepository<AnwserBlock> anwserBlockRepository;
        private IRepository<User> userRepository;
        private IRepository<SurveyReportData> surveyReportDataRepository;
        private IGenericRepository<Company> companyGenericRepository;
        private IConfiguration config;

        public UnitOfWork(ContextClass context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        IGenericRepository<Company> IUnitOfWork.companyGenericRepository
        {
            get
            {
                if (this.companyGenericRepository == null)
                {
                    this.companyGenericRepository = new GenericRepository<Company>("Company", "CompanyId", this.config);
                }

                return this.companyGenericRepository;
            }
        }

        //public IGenericRepository<Company> CompanyGenericRepository
        //{
        //    get
        //    {
        //        if (this.companyGenericRepository == null)
        //        {
        //            this.companyGenericRepository = new GenericRepository<Company>("Company", "CompanyId", this.config);
        //        }

        //        return this.companyGenericRepository;
        //    }
        //}

        public IRepository<SurveyReportData> SurveyReportDataRepository
        {
            get
            {
                if (this.surveyReportDataRepository == null)
                {
                    this.surveyReportDataRepository = new Repository<SurveyReportData>(this.context);
                }

                return this.surveyReportDataRepository;
            }
        }

        public IRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new Repository<User>(this.context);
                }

                return this.userRepository;
            }
        }

        public IRepository<Company> CompanyRepository
        {
            get
            {
                if (this.companyRepository == null)
                {
                    this.companyRepository = new Repository<Company>(this.context);
                }

                return this.companyRepository;
            }
        }

        public IRepository<Surveys> SurveysRepository
        {
            get
            {
                if (this.surveyRepository == null)
                {
                    this.surveyRepository = new Repository<Surveys>(this.context);
                }

                return this.surveyRepository;
            }
        }

        public IRepository<AnwserBlock> AnwserBlockRepository
        {
            get
            {
                if (this.anwserBlockRepository == null)
                {
                    this.anwserBlockRepository = new Repository<AnwserBlock>(this.context);
                }

                return this.anwserBlockRepository;
            }
        }

        public IRepository<Anwser> AnwserRepository
        {
            get
            {
                if (this.anwserRepository == null)
                {
                    this.anwserRepository = new Repository<Anwser>(this.context);
                }

                return this.anwserRepository;
            }
        }

        public IRepository<SurveyReport> SurveyReportRepository
        {
            get
            {
                if (this.surveyReportRepository == null)
                {
                    this.surveyReportRepository = new Repository<SurveyReport>(this.context);
                }

                return this.surveyReportRepository;
            }
        }

        public IRepository<Question> QuestionRepository
        {
            get
            {
                if (this.questionRepository == null)
                {
                    this.questionRepository = new Repository<Question>(this.context);
                }

                return this.questionRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
