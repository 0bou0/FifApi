using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_taille_tal")]
    public class Taille
    {
        [Key]
        [Column("tal_taille", TypeName = "char(6)")]
        public string IdTaille { get; set; }

        [Required]
        [Column("tal_nom")]
        [StringLength(50)]
        public string NomTaille { get; set; }

        [Column("tal_description")]
        [StringLength(100)]
        public string? DescriptionTaille { get; set; }  

        [InverseProperty(nameof(Stock.TailleDuProduit))]
        public virtual ICollection<Stock> StockDuProduit { get; set; } = null!;
    }
}
}
