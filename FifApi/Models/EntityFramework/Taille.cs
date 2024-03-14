using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_taille_tal")]
    public class Taille
    {
        [Key]
        [Column("idtaille", TypeName = "char(6)")]
        public string IdTaille { get; set; }

        [Required]
        [Column("nomtaille")]
        [StringLength(50)]
        public string NomTaille { get; set; }

        [Column("descriptiontaille")]
        [StringLength(100)]
        public string? DescriptionTaille { get; set; }  

        [InverseProperty("TailleDuProduit")]
        public virtual ICollection<Stock> StockDuProduit { get; set; } = null!;
    }
}
}
