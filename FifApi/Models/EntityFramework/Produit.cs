using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_produit_pdt")]
    public class Produit
    {
        [Key]
        [Column("pdt_id")]
        public int Id { get; set; }

        [Required]
        [Column("pdt_nom")]
        [StringLength(150)]
        public string Name { get; set; }

        [Column("pdt_description")]
        [StringLength(1000)]
        public string? Description { get; set; }

        [Column("pdt_caracteristiques")]
        [StringLength(500)]
        public string? Caracteristiques { get; set; }


        [InverseProperty(nameof(CouleurProduit.Produit_CouleurProduit))]
        public virtual ICollection<CouleurProduit> CouleursProduit { get; set; }
    }
}
