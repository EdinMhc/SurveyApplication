namespace Survey.API.Profiles
{
    using AutoMapper;
    using Survey.API.DTOs.QuestionDtos;
    using Survey.Infrastructure.Entities;

    public class QuestionConfiguration : Profile
    {
        public QuestionConfiguration()
        {
            CreateMap<Question, QuestionBasicInfoDto>();
            CreateMap<Survey.API.DTOs.QuestionDtos.QuestionBasicInfoDto, Survey.Infrastructure.Entities.Question>();

            CreateMap<Question, QuestionForCreationDto>();
            CreateMap<Survey.API.DTOs.QuestionDtos.QuestionForCreationDto, Survey.Infrastructure.Entities.Question>();

            CreateMap<Question, QuestionUpdateDto>();
            CreateMap<Survey.API.DTOs.QuestionDtos.QuestionUpdateDto, Survey.Infrastructure.Entities.Question>();
        }
    }
}
