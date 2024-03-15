using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_utilisateur_utl")]
    public class Utilisateur
    {
        [Key]
        [Column("utl_id")]
        public int IdUtilisateur { get; set; }

        [Required]
        [Column("utl_pseudo")]
        [StringLength(100)]
        public string PseudoUtilisateur { get; set; }

        [Required]
        [Column("utl_mdp")]
        public string MotDePasse { get; set; }

        [Column("utl_nom")]
        [StringLength(100)]
        public string? NomUtilisateur { get; set; }

        [Column("utl_prenom")]
        [StringLength(100)]
        public string? PrenomUtilisateur { get; set; }

        [Required]
        [Column("utl_email")]
        [StringLength(150)]
        public string MailUtilisateur { get; set; }

        [Required]
        [Column("utl_adresse")]
        public int IdAdresse { get; set; }


        [ForeignKey(nameof(IdUtilisateur))]
        [InverseProperty(nameof(Adresse.UtilisateurAdresse))]
        public virtual Adresse AdresseDeUtilisateur { get; set; } = null!;



        [InverseProperty(nameof(InfoCB.UtilisateurCB))]
        public virtual ICollection<InfoCB> CBDeUtilisateur { get; set; } = null!;


        [InverseProperty(nameof(Commande.UtilisateurCommande))]
        public virtual ICollection<Commande> CommandeDeUtilisateur { get; set; } = null!;

    }
}
