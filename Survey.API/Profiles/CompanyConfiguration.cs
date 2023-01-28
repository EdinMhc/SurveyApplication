namespace Survey.API.Profiles
{

    public class CompanyConfiguration : Profile
    {
        public CompanyConfiguration()
        {
            // Source --> Destination
            CreateMap<Company, CompanyDto>();
            CreateMap<Company, CompanyEditDto>();
            CreateMap<CompanyEditDto, CompanyFullInfoDto>();

            // ForMember(destination => destination.CompanyName, source => source.MapFrom(x => x.CompanyName));
            CreateMap<CompanyDto, Company>();
            CreateMap<CompanyEditDto, Company>();
            CreateMap<CompanyFullInfoDto, CompanyEditDto>();
        }
    }
}
