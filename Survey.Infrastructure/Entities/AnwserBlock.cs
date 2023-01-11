namespace Survey.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AnwserBlock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnwserBlockID { get; set; }

        [ForeignKey("CompanyId")]
        public int? CompanyID { get; set; }

        public int CodeOfAnwserBlock { get; set; }

        [MaxLength(255)]
        public string AnwserBlockName { get; set; }

        [MaxLength(30)]
        public string BlockType { get; set; }

        public Company Company { get; set; }

        public virtual ICollection<Anwser>? Anwsers { get; set; }

        public virtual ICollection<Question>? Questions { get; set; }
    }
}
