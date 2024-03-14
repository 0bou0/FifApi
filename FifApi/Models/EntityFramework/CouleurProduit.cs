using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_j_couleurproduit_clp")]
    public class CouleurProduit
    {
        [Key]
        [Column("clp_produit")]
        public int IdProduit { get; set; }

        [Key]
        [Column("clp_couleur")]
        public int IdCouleur { get; set; }

        [Required]
        [Column("clp_prix")]
        public decimal Prix { get; set; }

        [Column("clp_codebarre")]
        [StringLength(48)]
        public string? CodeBarre { get; set; }


        [ForeignKey(nameof(IdProduit))]
        [InverseProperty(nameof(Produit.CouleursProduit))]
        public virtual Produit Produit_CouleurProduit { get; set; } = null!;

        [ForeignKey(nameof(IdCouleur))]
        [InverseProperty(nameof(Couleur.CouleurProduits))]
        public virtual Couleur Couleur_CouleurProduit { get; set; } = null!;
    }
}
