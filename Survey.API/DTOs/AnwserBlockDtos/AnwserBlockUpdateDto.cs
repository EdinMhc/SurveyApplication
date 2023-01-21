﻿namespace Survey.API.DTOs.AnwserBlockDtos
{

    public class AnwserBlockUpdateDto
    {

        public int CodeOfAnwserBlock { get; set; }

        [MaxLength(255)]
        public string? AnwserBlockName { get; set; }

        [MaxLength(30)]
        public string? BlockType { get; set; }
    }
}
