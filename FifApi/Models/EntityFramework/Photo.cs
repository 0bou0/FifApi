using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_photo_pht")]
    public class Photo
    {
        [Key]
        [Column("pht_id")]
        public int IdPhoto { get; set; }

        [Required] 
        [Column("pht_url")]
        [StringLength(500)]
        public string URL { get; set; }

        [Column("pht_titre")]
        [StringLength(150)]
        public string? Titre { get; set; }

        [Column("pht_description")]
        [StringLength(250)]
        public string? Description { get; set; }


        [InverseProperty(nameof(AlbumPhoto.PhotoDesAlbums))]
        public virtual ICollection<AlbumPhoto> AlbumDesPhotos { get; set; } = null!;

    }
}
