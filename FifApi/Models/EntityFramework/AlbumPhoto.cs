using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_j_albumphoto_abp")]
    public class AlbumPhoto
    {

 

        [Key]
        [Column("abp_album")]
        public int IdAlbum { get; set; }

        [Key]
        [Column("abp_photo")]
        public int IdPhoto { get; set; }


        [ForeignKey(nameof(IdAlbum))]
        [InverseProperty(nameof(Album.AlbumDesPhotos))]
        public virtual Album AlbumPh { get; set; } = null!;

        [ForeignKey(nameof(IdPhoto))]
        [InverseProperty(nameof(Photo.AlbumDesPhotos))]
        public virtual Photo PhotoDesAlbums { get; set; } = null!;
    }
}
