using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_pays_pay")]
    public class Pays
    {
        [Key]
        [Column("pay_id", TypeName = "char(3)")]
        public string IdPays { get; set; }

        [Required]
        [Column("pay_nom")]
        [StringLength(75)]
        public string NomPays { get; set; }

        [InverseProperty(nameof(Ville.PaysVille))]
        public virtual ICollection<Ville> VilleDuPays { get; set; } = null!;

        [InverseProperty(nameof(Equipe.PaysDeEquipe))]
        public virtual ICollection<Equipe> EquipeDuPays { get; set; } = null!;


        [InverseProperty(nameof(Produit.PaysDuProduit))]
        public virtual ICollection<Produit> ProduitPays { get; set; } = null!;


    }
}
