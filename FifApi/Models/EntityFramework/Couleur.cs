using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_couleur_clr")]
    public class Couleur
    {
        [Key]
        [Column("clr_id")]
        public int Id { get; set; }

        [Required]
        [Column("clr_nom")]
        [StringLength(150)]
        public string Nom { get; set; }


        [InverseProperty(nameof(CouleurProduit.Couleur_CouleurProduit))]
        public virtual ICollection<CouleurProduit> CouleurProduits { get; set; } = null!;
    }
}
