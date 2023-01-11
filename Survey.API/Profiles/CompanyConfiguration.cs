namespace Survey.API.Profiles
{
    using AutoMapper;
    using Survey.API.DTOs.Company;
    using Survey.Infrastructure.Entities;

    public class CompanyConfiguration : Profile
    {
        public CompanyConfiguration()
        {
            // Source --> Destination
            this.CreateMap<Company, CompanyBasicInfoDto>();
            this.CreateMap<Company, CompanyCreationDto>();
            this.CreateMap<CompanyCreationDto, CompanyFullInfoDto>();

            // ForMember(destination => destination.CompanyName, source => source.MapFrom(x => x.CompanyName));
            this.CreateMap<CompanyBasicInfoDto, Survey.Infrastructure.Entities.Company>();
            this.CreateMap<CompanyCreationDto, Survey.Infrastructure.Entities.Company>();
            this.CreateMap<CompanyFullInfoDto, CompanyCreationDto>();
        }
    }
}
