namespace Survey.API.Profiles
{
    using AutoMapper;
    using Survey.API.DTOs.AnwserBlockDtos;
    using Survey.Infrastructure.Entities;

    public class AnwserBlockConfiguration : Profile
    {
        public AnwserBlockConfiguration()
        {
            this.CreateMap<AnwserBlock, AnwserBlockBasicInfoDto>();
            this.CreateMap<Survey.API.DTOs.AnwserBlockDtos.AnwserBlockBasicInfoDto, Survey.Infrastructure.Entities.AnwserBlock>();

            this.CreateMap<AnwserBlock, AnwserBlockForCreationDto>();
            this.CreateMap<Survey.API.DTOs.AnwserBlockDtos.AnwserBlockForCreationDto, Survey.Infrastructure.Entities.AnwserBlock>();

            this.CreateMap<AnwserBlock, AnwserBlockUpdateDto>();
            this.CreateMap<Survey.API.DTOs.AnwserBlockDtos.AnwserBlockUpdateDto, Survey.Infrastructure.Entities.AnwserBlock>();
        }
    }
}
