using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_j_sponsor_spc")]
    public class Sponsor
    {
        [Key]
        [Column("spc_equipe")]
        public int IdEquipe { get; set; }

        [Key]
        [Column("spc_marque")]
        public int IdMarque { get; set; }


        [ForeignKey(nameof(IdEquipe))]
        [InverseProperty(nameof(Equipe.SponsorMarque))]
        public virtual Equipe EquipeSponsorise { get; set; } = null!;

        [ForeignKey(nameof(IdMarque))]
        [InverseProperty(nameof(Marque.SponsorMarque))]
        public virtual Marque MarqueDuSponsor { get; set; } = null!;
    }
}
