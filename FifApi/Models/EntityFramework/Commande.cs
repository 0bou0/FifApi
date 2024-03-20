using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_commande_cmd")]
    public class Commande
    {
        [Key]
        [Column("cmd_id")]
        public int IdCommande { get; set; }

        [Column("cmd_utilisateur")]
        public int IdUtilisateur { get; set; }
        
        [Required]
        [Column("cmd_date", TypeName = "date")]
        public DateTime DateCommande { get; set; } = DateTime.Now;



        [ForeignKey(nameof(IdCommande))]
        [InverseProperty(nameof(Utilisateur.CommandeDeUtilisateur))]
        public virtual Utilisateur UtilisateurCommande { get; set; } = null!;

        [InverseProperty(nameof(LigneCommande.CommandeLigne))]
        public virtual ICollection<LigneCommande> LigneDeLaCommande { get; set; } = null!;
    }
}
