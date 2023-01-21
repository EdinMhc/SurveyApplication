namespace Survey.API.DTOs.AnwserDtos
{
    public class AnswerUpdateDto
    {
        [MaxLength(255)]
        public string? AnwserText { get; set; }
    }
}
