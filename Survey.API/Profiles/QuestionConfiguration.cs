namespace Survey.API.Profiles
{
    using AutoMapper;
    using Survey.API.DTOs.QuestionDtos;
    using Survey.Infrastructure.Entities;

    public class QuestionConfiguration : Profile
    {
        public QuestionConfiguration()
        {
            this.CreateMap<Question, QuestionBasicInfoDto>();
            this.CreateMap<Survey.API.DTOs.QuestionDtos.QuestionBasicInfoDto, Survey.Infrastructure.Entities.Question>();

            this.CreateMap<Question, QuestionForCreationDto>();
            this.CreateMap<Survey.API.DTOs.QuestionDtos.QuestionForCreationDto, Survey.Infrastructure.Entities.Question>();

            this.CreateMap<Question, QuestionUpdateDto>();
            this.CreateMap<Survey.API.DTOs.QuestionDtos.QuestionUpdateDto, Survey.Infrastructure.Entities.Question>();
        }
    }
}
