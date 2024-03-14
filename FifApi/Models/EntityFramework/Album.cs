using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_j_album_alb")]
    public class Album
    {
        [Key]
        [Column("alb_id")]
        public int IdAlbum { get; set; }


        [Column("alb_nom")]
        public string? NomAlbum { get; set; }

        [InverseProperty(nameof(Produit.AlbumDuProduit))]
        public virtual ICollection<Produit> ProduitAlbum { get; set; } = null!;


    }
}
