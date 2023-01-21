namespace Survey.API.Profiles
{

    public class CompanyConfiguration : Profile
    {
        public CompanyConfiguration()
        {
            // Source --> Destination
            CreateMap<Company, CompanyBasicInfoDto>();
            CreateMap<Company, CompanyCreationDto>();
            CreateMap<CompanyCreationDto, CompanyFullInfoDto>();

            // ForMember(destination => destination.CompanyName, source => source.MapFrom(x => x.CompanyName));
            CreateMap<CompanyBasicInfoDto, Survey.Infrastructure.Entities.Company>();
            CreateMap<CompanyCreationDto, Survey.Infrastructure.Entities.Company>();
            CreateMap<CompanyFullInfoDto, CompanyCreationDto>();
        }
    }
}
