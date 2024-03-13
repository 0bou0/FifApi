using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_typeproduit_tpd")]
    public class TypeProduit
    {
        [Key]
        [Column("tpd_id")]
        public int Id { get; set; }

        [Required]
        [Column("tpd_nom")]
        [StringLength(100)]
        public string Nom { get; set; }

        [Column("tpd_description")]
        [StringLength(150)]
        public string? Description { get; set; }

        [Column("tpd_surtype")]
        public int? IdSurType { get; set; }


        [ForeignKey(nameof(IdSurType))]
        [InverseProperty(nameof(TypeProduit.SousTypes))]
        public virtual TypeProduit SurType { get; set; } = null!;

        [InverseProperty(nameof(TypeProduit.SurType))]
        public virtual ICollection<TypeProduit> SousTypes { get; set; }
    }
}