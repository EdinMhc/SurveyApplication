using Survey.Domain.DapperServices.DapperCompanyServices;
using Survey.Infrastructure.DapperRepository;

namespace Survey.API.Dapper
{
    public static class ServiceRegistration
    {
        public static void AddDapperDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICompanyServiceDapper, CompanyServiceDapper>();
        }
    }
}
