namespace Survey.API.Dapper.DapperProfiles
{
    using AutoMapper;
    using Survey.API.Dapper.DapperDtoCompany;
    using Survey.Infrastructure.Entities;

    public sealed class DapperCompanyProfiles : Profile
    {
        public DapperCompanyProfiles()
        {
            //this.CreateMap<Company, DapperCompanyCreationDto>();
            //this.CreateMap<DapperCompanyCreationDto, Company>();

            this.CreateMap<Company, DapperCompanyUpdateDto>();
            this.CreateMap<DapperCompanyUpdateDto, Company>();
        }
    }
}
