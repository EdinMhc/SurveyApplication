namespace Survey.API.Profiles
{

    public class AnwserBlockConfiguration : Profile
    {
        public AnwserBlockConfiguration()
        {
            CreateMap<AnwserBlock, AnwserBlockBasicInfoDto>();
            CreateMap<Survey.API.DTOs.AnwserBlockDtos.AnwserBlockBasicInfoDto, Survey.Infrastructure.Entities.AnwserBlock>();

            CreateMap<AnwserBlock, AnwserBlockForCreationDto>();
            CreateMap<Survey.API.DTOs.AnwserBlockDtos.AnwserBlockForCreationDto, Survey.Infrastructure.Entities.AnwserBlock>();

            CreateMap<AnwserBlock, AnwserBlockUpdateDto>();
            CreateMap<Survey.API.DTOs.AnwserBlockDtos.AnwserBlockUpdateDto, Survey.Infrastructure.Entities.AnwserBlock>();
        }
    }
}
