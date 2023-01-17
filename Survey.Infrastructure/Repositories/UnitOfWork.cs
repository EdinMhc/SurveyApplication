namespace Survey.Infrastructure.Repositories
{
    using Microsoft.Extensions.Configuration;
    using Survey.Infrastructure.ContextClass1;
    using Survey.Infrastructure.DapperRepository;
    using Survey.Infrastructure.Entities;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContextClass _context;
        private readonly IConfiguration _config;
        private IRepository<Company> _companyRepository;
        private IRepository<Surveys> _surveyRepository;
        private IRepository<Question> _questionRepository;
        private IRepository<SurveyReport> _surveyReportRepository;
        private IRepository<Anwser> _anwserRepository;
        private IRepository<AnwserBlock> _anwserBlockRepository;
        private IRepository<User> _userRepository;
        private IRepository<SurveyReportData> _surveyReportDataRepository;
        private IGenericRepository<Company> _companyGenericRepository;

        public UnitOfWork(ContextClass context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        IGenericRepository<Company> IUnitOfWork.companyGenericRepository
        {
            get
            {
                if (_companyGenericRepository == null)
                {
                    _companyGenericRepository = new GenericRepository<Company>("Company", "CompanyId", _config);
                }

                return _companyGenericRepository;
            }
        }

        public IRepository<SurveyReportData> SurveyReportDataRepository
        {
            get
            {
                if (_surveyReportDataRepository == null)
                {
                    _surveyReportDataRepository = new Repository<SurveyReportData>(_context);
                }

                return _surveyReportDataRepository;
            }
        }

        public IRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new Repository<User>(_context);
                }

                return _userRepository;
            }
        }

        public IRepository<Company> CompanyRepository
        {
            get
            {
                if (_companyRepository == null)
                {
                    _companyRepository = new Repository<Company>(_context);
                }

                return _companyRepository;
            }
        }

        public IRepository<Surveys> SurveysRepository
        {
            get
            {
                if (_surveyRepository == null)
                {
                    _surveyRepository = new Repository<Surveys>(_context);
                }

                return _surveyRepository;
            }
        }

        public IRepository<AnwserBlock> AnwserBlockRepository
        {
            get
            {
                if (_anwserBlockRepository == null)
                {
                    _anwserBlockRepository = new Repository<AnwserBlock>(_context);
                }

                return _anwserBlockRepository;
            }
        }

        public IRepository<Anwser> AnwserRepository
        {
            get
            {
                if (_anwserRepository == null)
                {
                    _anwserRepository = new Repository<Anwser>(_context);
                }

                return _anwserRepository;
            }
        }

        public IRepository<SurveyReport> SurveyReportRepository
        {
            get
            {
                if (_surveyReportRepository == null)
                {
                    _surveyReportRepository = new Repository<SurveyReport>(_context);
                }

                return _surveyReportRepository;
            }
        }

        public IRepository<Question> QuestionRepository
        {
            get
            {
                if (_questionRepository == null)
                {
                    _questionRepository = new Repository<Question>(_context);
                }

                return _questionRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
