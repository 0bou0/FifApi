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


        [Column("pdt_marque", TypeName = "int")]
        public int MarqueId { get; set; }

        [Column("pdt_type", TypeName = "int")]
        public int TypeId { get; set; }

        [Column("pdt_album", TypeName = "int")]
        public int AlbumId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        [InverseProperty(nameof(Album.ProduitAlbum))]
        public virtual Album AlbumDuProduit { get; set; } = null!;

        [ForeignKey(nameof(MarqueId))]
        [InverseProperty(nameof(Marque.ProduitMarque))]
        public virtual Marque MarqueduProduit { get; set; } = null!;

        [ForeignKey(nameof(TypeId))]
        [InverseProperty(nameof(TypeProduit.SurType))]
        public virtual TypeProduit TypePourLeProduit { get; set; } = null!;


        [InverseProperty(nameof(CouleurProduit.Produit_CouleurProduit))]
        public virtual ICollection<CouleurProduit> CouleursProduits { get; set; } = null!;
    }
}
