using System.ComponentModel.DataAnnotations;

namespace Survey.API.DTOs.AnwserDtos
{
    public class AnswerBasicInfoDto
    {
        public int AnwserID { get; set; }

        public int AnwserBlockID { get; set; }

        [MaxLength(255)]
        public string AnwserText { get; set; }
    }
}
