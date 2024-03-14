using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_stock_stc")]
    public class Stock
    {
        [Key]
        [Column("stc_taille", TypeName = "char(6)")]
        public string TailleId { get; set; }

        [Required]
        [Column("stc_quantite")]
        public int Quantite { get; set; }


        [Key]
        [Column("stc_couleurproduit")]
        public int CouleurProduitId { get; set; }

        [ForeignKey(nameof(TailleId))]
        [InverseProperty(nameof(Taille.StockDuProduit))]
        public virtual Taille TailleDuProduit { get; set; }

        [ForeignKey(nameof(CouleurProduitId))]
        [InverseProperty(nameof(CouleurProduit.ProduitStock))]
        public virtual CouleurProduit ProduitEncouleuret { get; set; }
    }
}
