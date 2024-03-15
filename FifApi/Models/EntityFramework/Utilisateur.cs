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

        [Column("utl_pseudo")]
        [StringLength(100)]
        public string PseudoUtilisateur { get; set; }

        [Column("utl_nom")]
        [StringLength(100)]
        public string NomUtilisateur { get; set; }

        [Column("utl_prenom")]
        [StringLength(100)]
        public string PrenomUtilisateur { get; set; }

        [Column("utl_email")]
        [StringLength(150)]
        public string MailUtilisateur { get; set; }
    }
}
