using System.ComponentModel.DataAnnotations;

namespace Survey.API.DTOs.AnwserDtos
{
    public class AnswerForCreationDto
    {
        [MaxLength(255)]
        public string AnwserText { get; set; }
    }
}
