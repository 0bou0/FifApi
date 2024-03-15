using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_j_lignecommande_lcm")]
    public class LigneCommande
    {
        [Key]
        [Column("lcm_commande")]
        public int IdCommande { get; set; }

        [Key]
        [Column("lcm_stock")]
        public int IdStock { get; set; }


        [ForeignKey(nameof(IdStock))]
        [InverseProperty(nameof(Stock.LigneDuStock))]
        public virtual Stock StockLigneCommande { get; set; } = null!;


        [ForeignKey(nameof(IdCommande))]
        [InverseProperty(nameof(Commande.LigneDeLaCommande))]
        public virtual Commande CommandeLigne { get; set; } = null!;
    }
}
