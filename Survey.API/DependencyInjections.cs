using Survey.Domain.Services;
using Survey.Domain.Services.IdentityService;
using Survey.Domain.Services.IdentityService.Interfaces;
using Survey.Domain.Services.Interfaces;
using Survey.Infrastructure.Repositories;

namespace Survey.API
{
    public static class DependencyInjections
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<ISurveyReportService, SurveyReportService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnwserBlockService, AnwserBlockService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<ISurveyReportDataService, SurveyReportDataService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddHttpContextAccessor();
        }
    }
}
