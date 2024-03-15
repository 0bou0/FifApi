using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_adresse_adr")]
    public class Adresse
    {
        [Key]
        [Column("adr_id")]
        public int IdAdresse { get; set; }

        [Required]
        [Column("adr_codepostal", TypeName ="char(15)")]
        public string CodePostal { get; set; }


        [Column("adr_rue")]
        [StringLength(250)]
        public string? Rue { get; set; }

        [Column("adr_numrue")]
        public int? NumRue { get; set; }

        [Column("adr_long", TypeName = "numeric(20,15)")]
        public decimal? Longitude { get; set; }

        [Column("adr_lat", TypeName = "numeric(20,15)")]
        public decimal? Lattitude { get; set; }

        [Required]
        [Column("adr_ville")]
        public int IdVille { get; set; }


        [InverseProperty(nameof(Utilisateur.AdresseDeUtilisateur))]
        public virtual ICollection<Utilisateur> UtilisateurAdresse { get; set; } = null!;


        [ForeignKey(nameof(IdVille))]
        [InverseProperty(nameof(Ville.AdresseDeLaVille))]
        public virtual Ville VilleAdresse { get; set; } = null!;
    }
}
