using AutoMapper;
using Survey.API.DTOs.AnwserDtos;
using Survey.Infrastructure.Entities;

namespace Survey.API.Profiles
{
    public class AnwserConfiguration : Profile
    {

        public AnwserConfiguration()
        {
            this.CreateMap<Anwser, AnswerBasicInfoDto>();
            this.CreateMap<Survey.API.DTOs.AnwserDtos.AnswerBasicInfoDto, Survey.Infrastructure.Entities.Anwser>();

            this.CreateMap<Anwser, AnswerForCreationDto>();
            this.CreateMap<Survey.API.DTOs.AnwserDtos.AnswerForCreationDto, Survey.Infrastructure.Entities.Anwser>();

            this.CreateMap<Anwser, AnswerUpdateDto>();
            this.CreateMap<Survey.API.DTOs.AnwserDtos.AnswerUpdateDto, Survey.Infrastructure.Entities.Anwser>();
        }
    }
}
