using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_pays_pay")]
    public class Pays
    {
        [Key]
        [Column("pay_id")]
        public string IdPays { get; set; }

        
        [Column("pay_nom")]
        [StringLength(75)]
        public int? NomPays { get; set; }

        [InverseProperty(nameof(Ville.PaysVille))]
        public virtual ICollection<Ville> VilleDuPays { get; set; } = null!;

        [InverseProperty(nameof(Equipe.PaysDeEquipe))]
        public virtual ICollection<Equipe> EquipeDuPays { get; set; } = null!;


    }
}
