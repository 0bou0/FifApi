using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_stock_stc")]
    public class Stock
    {
        [Required]
        [Column("stc_taille", TypeName = "char(6)")]
        public string TailleId { get; set; }

        [Required]
        [Column("stc_quantite")]
        public int Quantite { get; set; }

        [Required]
        [Column("stc_couleurproduit")]
        public int CouleurProduitId { get; set; }

        [Key]
        [Column("stc_id")]
        public int IdStock { get; set; }


        [ForeignKey(nameof(TailleId))]
        [InverseProperty(nameof(Taille.StockDuProduit))]
        public virtual Taille TailleDuProduit { get; set; }

        [ForeignKey(nameof(CouleurProduitId))]
        [InverseProperty(nameof(CouleurProduit.ProduitStock))]
        public virtual CouleurProduit ProduitEncouleur { get; set; }

        [InverseProperty(nameof(LigneCommande.StockLigneCommande))]
        public virtual ICollection<LigneCommande> LigneDuStock { get; set; } = null!;

    }
}
