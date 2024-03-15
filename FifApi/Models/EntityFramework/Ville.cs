using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_ville_vil")]
    public class Ville
    {

        [Key]
        [Column("vil_id")]
        public int IdVille { get; set; }

        [Column("vil_nom")]
        [StringLength(50)]
        public string? NomVille { get; set; }

        [Column("vil_numdep", TypeName="char(5,0)")]
        public string? NumDep { get; set; }


        [Required]
        [Column("vil_pays", TypeName = "char(5,0)")]
        public string IdPays { get; set; }

        [InverseProperty(nameof(Adresse.VilleAdresse))]
        public virtual ICollection<Adresse> AdresseDeLaVille { get; set; } = null!;


        [ForeignKey(nameof(IdPays))]
        [InverseProperty(nameof(Pays.VilleDuPays))]
        public virtual Pays PaysVille { get; set; } = null!;
    }
}
