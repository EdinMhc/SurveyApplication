using System.ComponentModel.DataAnnotations;

namespace Survey.API.DTOs.QuestionDtos
{
    public class QuestionForCreationDto
    {
        [Required]
        public int AnwserBlockID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string QuestionText { get; set; }

        [Required]
        [MaxLength(255)]
        public string QuestionType { get; set; }
    }
}
