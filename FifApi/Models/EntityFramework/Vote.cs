using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_j_vote_vot")]
    public class Vote
    {

        [Key]
        [Column("vot_joueur")]
        public int IdJoueur { get; set; }

        [Key]
        [Column("vot_utilisateur")]
        public int IdUtilisateur { get; set;}


        [ForeignKey(nameof(IdJoueur))]
        [InverseProperty(nameof(Joueur.VotePourJoueur))]
        public virtual Joueur JoueurVoter { get; set; } = null!;

        [ForeignKey(nameof(IdUtilisateur))]
        [InverseProperty(nameof(Utilisateur.VoteDeUtilisateur))]
        public virtual Utilisateur UtilisateurVote { get; set; } = null!;
    }
}
