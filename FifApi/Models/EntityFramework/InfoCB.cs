using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_infocb_icb")]
    public class InfoCB
    {
        [Key]
        [Column("icb_id")]
        public int IdCB { get; set; }

        [Required]
        [Column("icb_numcb", TypeName = "numeric(16,0)")]
        public decimal NumeroCB { get; set; }

        [Required]
        [Column("icb_cryptogramme", TypeName = "numeric(3,0)")]
        public decimal Cryptogramme { get; set; }

        [Required]
        [Column("icb_dateexpiration", TypeName = "date")]
        public DateTime DateExpiration { get; set; }

        [Required]
        [Column("icb_utilisateur")]
        public int UtilisateurId { get; set; }




        [ForeignKey(nameof(UtilisateurId))]
        [InverseProperty(nameof(Utilisateur.CBDeUtilisateur))]
        public virtual Utilisateur UtilisateurCB { get; set; } = null!;
    }
}
