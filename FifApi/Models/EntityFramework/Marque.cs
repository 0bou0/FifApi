using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_marque_mrq")]
    public class Marque
    {
        [Key]
        [Column("mrq_id")]
        public int IdMarque { get; set; }

        [Required]
        [Column("mrq_nom")]
        [StringLength(200)]
        public string NomMarque { get; set; }


        [InverseProperty("MarqueduProduit")]
        public virtual ICollection<Produit> ProduitMarque { get; set; } = null!;
    }
}
