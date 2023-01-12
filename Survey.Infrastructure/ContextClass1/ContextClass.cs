namespace Survey.Infrastructure.ContextClass1
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Survey.Infrastructure.ContextClass1.Configurations;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Entities.JwtRelated;

    public class ContextClass : IdentityDbContext<User>
    {
        public ContextClass(DbContextOptions options)
            : base(options)
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
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
            this.SeedUser(modelBuilder);
            //this.SeedCompanies(modelBuilder);
            //this.SeedSurveys(modelBuilder);
            //this.SeedSurveyReport(modelBuilder);
            //this.SeedAnwserBlock(modelBuilder);
            //this.SeedQuestion(modelBuilder);
            //this.SeedAnwsers(modelBuilder);
        }

        private void SeedUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                UsersToSeed[0]);
        }

        // Using a List of users to seed a object to Company, at the same time info for SeedUser.
        private List<User> UsersToSeed = new List<User>
        {
            new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "edinmuhic00@gmail.com",
                    FirstName = "Edin",
                    LastName = "Muhic",
                    PasswordHash = "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==",
                    Email = "edinmuhic00@gmail.com",
                    NormalizedUserName = "EDINMUHIC00@GMAIL.COM",
                    NormalizedEmail = "EDINMUHIC00@GMAIL.COM",
                }
        };

        private void SeedCompanies(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    CompanyID = 1,
                    User = this.UsersToSeed[0],
                    CompanyName = "BINGO",
                    Address = "STUPINE",
                    Email = "bingo",
                    CreateDate = DateTime.Now,
                },
                new Company
                {
                    CompanyID = 2,
                    User = this.UsersToSeed[0],
                    CompanyName = "Forsta",
                    Address = "Maglajska 1.br",
                    Email = "forsta@forsta.com",
                    CreateDate = DateTime.Now,
                });
        }

        //private void SeedSurveys(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Surveys>().HasData(
        //    new Surveys
        //    {
        //        SurveyID = 1,
        //        CreatedBy = "Forsta",
        //        IsActive = true,
        //        CreateDate = DateTime.Now,
        //        SurveyName = "Are You Okay?",
        //        CompanyID = 1,
        //    },
        //    new Surveys
        //    {
        //        SurveyID = 2,
        //        CreatedBy = "Microsoft",
        //        IsActive = false,
        //        CreateDate = DateTime.Now,
        //        SurveyName = "Software Satisfaction?",
        //        CompanyID = 2,
        //    },
        //    new Surveys
        //    {
        //        SurveyID = 3,
        //        CreatedBy = "Tesla",
        //        IsActive = true,
        //        CreateDate = DateTime.Now,
        //        SurveyName = "Car Working?",
        //        CompanyID = 3,
        //    });
        //}

        //private void SeedSurveyReport(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<SurveyReport>().HasData(
        //        new SurveyReport
        //        {
        //            SurveyReportID = 4,
        //            SurveyID = 4,
        //            IsCompleted = true,
        //            CreateDate = DateTime.Now,
        //        },
        //        new SurveyReport
        //        {
        //            SurveyReportID = 5,
        //            SurveyID = 5,
        //            IsCompleted = false,
        //            CreateDate = DateTime.Now,
        //        });
        //}

        //private void SeedAnwserBlock(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<AnwserBlock>().HasData(
        //    new AnwserBlock
        //    {
        //        AnwserBlockID = 1,
        //        AnwserBlockName = "Anwsers For Questions",
        //        BlockType = "Text",
        //        CodeOfAnwserBlock = 1,
        //    },
        //    new AnwserBlock
        //    {
        //        AnwserBlockID = 2,
        //        AnwserBlockName = "Anwsers For Questions",
        //        BlockType = "Multiple choice",
        //        CodeOfAnwserBlock = 2,
        //    });
        //}

        //private void SeedQuestion(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Question>().HasData(
        //        new Question
        //        {
        //            QuestionID = 4,
        //            SurveyID = 4,
        //            AnwserBlockID = 1,
        //            Code = "dgazu1231asgdzr5123",
        //            QuestionText = "Question1?",
        //            QuestionType = "MultipleChoice",
        //        },
        //        new Question
        //        {
        //            QuestionID = 5,
        //            SurveyID = 4,
        //            AnwserBlockID = 1,
        //            Code = "gaisd123216rd721z56",
        //            QuestionText = "Question2?",
        //            QuestionType = "MultipleChoice",
        //        },
        //        new Question
        //        {
        //            QuestionID = 6,
        //            AnwserBlockID = 1,
        //            SurveyID = 5,
        //            Code = "d21thgei21567gr5",
        //            QuestionText = "Question3?",
        //            QuestionType = "MultipleChoice",
        //        },
        //        new Question
        //        {
        //            QuestionID = 7,
        //            AnwserBlockID = 1,
        //            SurveyID = 5,
        //            Code = "6t7216r7fe56r5r7g",
        //            QuestionText = "Question4?",
        //            QuestionType = "MultipleChoice",
        //        },
        //        new Question
        //        {
        //            QuestionID = 8,
        //            AnwserBlockID = 1,
        //            SurveyID = 6,
        //            Code = "7gehi378gdf67gi26t732",
        //            QuestionText = "Question5?",
        //            QuestionType = "MultipleChoice",
        //        },
        //        new Question
        //        {
        //            QuestionID = 9,
        //            AnwserBlockID = 1,
        //            SurveyID = 6,
        //            Code = "d6t73zdhufg7364gt73",
        //            QuestionText = "Question4?",
        //            QuestionType = "MultipleChoice",
        //        },
        //        new Question
        //        {
        //            QuestionID = 10,
        //            AnwserBlockID = 2,
        //            SurveyID = 4,
        //            Code = "g318267gd332irt362tgdz3uidzuhdsgf",
        //            QuestionText = "Question Text1?",
        //            QuestionType = "Text",
        //        },
        //        new Question
        //        {
        //            QuestionID = 11,
        //            AnwserBlockID = 2,
        //            SurveyID = 4,
        //            Code = "t672ge1huiegei66713wdas",
        //            QuestionText = "Question Text1?",
        //            QuestionType = "Text",
        //        }
        //        );
        //}

        //private void SeedAnwsers(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Anwser>().HasData(
        //        new Anwser
        //        {
        //            AnwserBlockID = 1,
        //            AnwserText = "The anwser for question1",
        //            AnwserID = 4,
        //        },
        //        new Anwser
        //        {
        //            AnwserBlockID = 1,
        //            AnwserText = "The anwser for question2",
        //            AnwserID = 5,
        //        },
        //        new Anwser
        //        {
        //            AnwserBlockID = 1,
        //            AnwserText = "The anwser for question3",
        //            AnwserID = 6,
        //        },
        //        new Anwser
        //        {
        //            AnwserBlockID = 1,
        //            AnwserText = "The anwser for question4",
        //            AnwserID = 7,
        //        },
        //        new Anwser
        //        {
        //            AnwserBlockID = 1,
        //            AnwserText = "The anwser for question5",
        //            AnwserID = 8,
        //        },
        //        new Anwser
        //        {
        //            AnwserBlockID = 2,
        //            AnwserText = "The anwser for Text1",
        //            AnwserID = 9,
        //        },
        //        new Anwser
        //        {
        //            AnwserBlockID = 2,
        //            AnwserText = "The anwser for Text2",
        //            AnwserID = 10,
        //        },
        //        new Anwser
        //        {
        //            AnwserBlockID = 2,
        //            AnwserText = "The anwser for Text2",
        //            AnwserID = 11,
        //        });
        //}
    }
}