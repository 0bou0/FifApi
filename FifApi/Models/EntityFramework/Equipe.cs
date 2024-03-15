using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_equipe_eqp")]
    public class Equipe
    {
        [Key]
        [Column("eqp_id")]
        public int IdEquipe { get; set; }

        [Required] 
        [Column("eqp_nom")]
        [StringLength(100)]
        public string NomEquipe { get; set; }

        [Column("eqp_histoire")]
        [StringLength(500)]
        public string? HistoireEquipe { get; set; }


        [Required]
        [Column("eqp_pays")]
        public string IdPays { get; set; }



        [InverseProperty(nameof(Sponsor.EquipeSponsorise))]
        public virtual ICollection<Sponsor> SponsorMarque { get; set; } = null!;



        [ForeignKey(nameof(IdPays))]
        [InverseProperty(nameof(Pays.EquipeDuPays))]
        public virtual Pays PaysDeEquipe { get; set; } = null!;
        

        [InverseProperty(nameof(Match.EquipeEnMatch))]
        public virtual ICollection<Match> MatchEnEquipe { get; set; } = null!;
    }
}
