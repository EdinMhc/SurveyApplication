namespace Survey.API.Dapper.DapperProfiles
{
    using AutoMapper;
    using Survey.API.Dapper.DapperDtoCompany;
    using Survey.Infrastructure.Entities;

    public sealed class DapperCompanyProfiles : Profile
    {
        public DapperCompanyProfiles()
        {
            CreateMap<Company, DapperCompanyUpdateDto>();
            CreateMap<DapperCompanyUpdateDto, Company>();
        }
    }
}
