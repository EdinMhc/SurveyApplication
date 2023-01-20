using AutoMapper;
using Survey.API.DTOs.AnwserDtos;
using Survey.Infrastructure.Entities;

namespace Survey.API.Profiles
{
    public class AnwserConfiguration : Profile
    {

        public AnwserConfiguration()
        {
            CreateMap<Anwser, AnswerBasicInfoDto>();
            CreateMap<Survey.API.DTOs.AnwserDtos.AnswerBasicInfoDto, Survey.Infrastructure.Entities.Anwser>();

            CreateMap<Anwser, AnswerForCreationDto>();
            CreateMap<Survey.API.DTOs.AnwserDtos.AnswerForCreationDto, Survey.Infrastructure.Entities.Anwser>();

            CreateMap<Anwser, AnswerUpdateDto>();
            CreateMap<Survey.API.DTOs.AnwserDtos.AnswerUpdateDto, Survey.Infrastructure.Entities.Anwser>();
        }
    }
}
