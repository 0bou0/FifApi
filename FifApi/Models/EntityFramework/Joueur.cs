using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_joueur_jor")]
    public class Joueur
    {
        [Key]
        [Column("jor_id")]
        public int IdJoueur { get; set; }

        [Required]
        [Column("jor_nom")]
        [StringLength(150)]
        public string NomJoueur { get; set; }

        [Required]
        [Column("prenomjoueur")]
        [StringLength(150)]
        public string PrenomJoueur { get; set; }

        [Required]
        [Column("jor_sexe", TypeName = "Char(1)")]
        public string SexeJoueur { get; set; }

        [Column("jor_datenaissance", TypeName = "Date")]
        public DateTime? DateNaissanceJoueur { get; set; }

        [Column("jor_datedece", TypeName = "Date")]
        public DateTime? DateDecesJoueur { get; set; }

        [Column("jor_debutcarriere", TypeName = "Date")]
        public DateTime? DebutCarriereJoueur { get; set; }

        [Column("jor_fincarriere", TypeName = "Date")]
        public DateTime? FinCarriereJoueur { get; set; }


        [Column("jor_description")]
        [StringLength(1000)]
        public string? DescriptionJoueur { get; set; }

        [Required]
        [Column("posteid", TypeName = "int")]
        public int PosteId { get; set; }

        [InverseProperty(nameof(JoueurMatch.JoueurDansMatch))]
        public virtual ICollection<JoueurMatch> JouabiliteMatch { get; set; } = null!;

        [InverseProperty(nameof(Vote.JoueurVoter))]
        public virtual ICollection<Vote> VotePourJoueur { get; set; } = null!;

        [ForeignKey(nameof(PosteId))]
        [InverseProperty(nameof(Poste.JoueurPoste))]
        public virtual Poste PostePourJoueur { get; set; } = null!;

    }
}
