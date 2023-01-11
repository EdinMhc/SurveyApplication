namespace Survey.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///
    /// </summary>
    public class Anwser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnwserID { get; set; }

        [ForeignKey("AnwserBlockID")]
        public int AnwserBlockID { get; set; }

        [MaxLength(255)]
        public string AnwserText { get; set; }

        public virtual AnwserBlock AnwserBlock { get; set; }
    }
}
