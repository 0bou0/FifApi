using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_j_joueurmatch_jrm")]
    public class JoueurMatch
    {
        [Key]
        [Column("jrm_joueur")]
        public int JoueurId { get; set; }

        [Key]
        [Column("jrm_match")]
        public int MatchId { get; set; }

        [Required]
        [Column("jrm_nbbut")]
        public int NbButs { get; set; }


        [ForeignKey("JoueurId")]
        [InverseProperty("JouabiliteMatch")]
        public virtual Joueur JoueurDansMatch { get; set; } = null!;
        [ForeignKey("MatchId")]
        [InverseProperty("JouabiliteMatch")]
        public virtual Match MatchPourJoueur { get; set; } = null!;
    }
}
