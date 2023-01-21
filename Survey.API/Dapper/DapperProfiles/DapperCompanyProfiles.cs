namespace Survey.API.Dapper.DapperProfiles
{

    public sealed class DapperCompanyProfiles : Profile
    {
        public DapperCompanyProfiles()
        {
            CreateMap<Company, DapperCompanyUpdateDto>();
            CreateMap<DapperCompanyUpdateDto, Company>();
        }
    }
}
