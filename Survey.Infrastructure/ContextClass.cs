using Survey.Infrastructure.Configurations;
using Survey.Infrastructure.Entities;
using Survey.Infrastructure.Entities.JwtRelated;

namespace Survey.Infrastructure
{

    public class ContextClass : IdentityDbContext<User>
    {
        public ContextClass(DbContextOptions options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<Company> Company { get; set; }

        public DbSet<Surveys> Survey { get; set; }

        public DbSet<SurveyReport> SurveyReport { get; set; }

        public DbSet<Question> Question { get; set; }

        public DbSet<AnwserBlock> AnwserBlock { get; set; }

        public DbSet<Anwser> Anwser { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<SurveyReportData> surveyReportData { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionCascadeDelete());
            modelBuilder.ApplyConfiguration(new OnDeleteConf());
            modelBuilder.ApplyConfiguration(new SurveyCascadeDelete());
            modelBuilder.ApplyConfiguration(new CompanyCascadeDelete());
            modelBuilder.ApplyConfiguration(new AswerCascadeDelete());
            modelBuilder.ApplyConfiguration(new AnswerBlockCascadeDelete());
            SeedUser(modelBuilder);
        }

        private void SeedUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                UsersToSeed[0]);
        }

        private List<User> UsersToSeed = new List<User>
        {
            new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "a@a",
                    FirstName = "Edin",
                    LastName = "Muhic",
                    PasswordHash = "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==",
                    Email = "A@A",
                    NormalizedUserName = "A@A",
                    NormalizedEmail = "A@A",
                }
        };
    }
}