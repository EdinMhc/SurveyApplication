using System.ComponentModel.DataAnnotations;

namespace Survey.API.DTOs.QuestionDtos
{
    public class QuestionUpdateDto
    {
        public int? AnwserBlockID { get; set; }

        [MaxLength(255)]
        public string? Code { get; set; }

        [MaxLength(100)]
        public string? QuestionText { get; set; }

        [MaxLength(255)]
        public string? QuestionType { get; set; }
    }
}
