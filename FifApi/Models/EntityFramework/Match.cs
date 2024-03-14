using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_match_mch")]
    public class Match
    {
        [Key]
        [Column("mch_id")]
        public int IdMatch { get; set; }

        [Column("mch_scoreint")]
        public int? ScoreEquipeDomicile { get; set; }
        

        [Column("mch_scoreext")]
        public int? ScoreEquipeExterieure { get; set; }

        [Column("mch_nom")]
        [StringLength(50)]
        public string? NomMatch { get; set; }

        [Column("mch_date", TypeName = "Date")]
        public DateTime? DateMatch { get; set; }

        [InverseProperty("MatchPourJoueur")]
        public virtual ICollection<JoueurMatch> JouabiliteMatch { get; set; } = null!;
    }
}
