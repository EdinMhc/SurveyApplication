namespace Survey.API.DTOs.AnwserBlockDtos
{
    using System.ComponentModel.DataAnnotations;

    public class AnwserBlockUpdateDto
    {

        public int CodeOfAnwserBlock { get; set; }

        [MaxLength(255)]
        public string? AnwserBlockName { get; set; }

        [MaxLength(30)]
        public string? BlockType { get; set; }
    }
}
