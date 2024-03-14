using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_poste_pst")]
    public class Poste
    {
        [Key]
        [Column("pst_id")]
        public int Idposte { get; set; }

        [Required]
        [Column("pst_nom")]
        [StringLength(150)]
        public string NomPoste { get; set; }

        [Column("pst_description")]
        [StringLength(300)]
        public string? DescriptionPoste { get; set; }

        [InverseProperty(nameof(Joueur.PosteId))]
        public virtual ICollection<Joueur> JoueurPoste { get; set; } = null!;
    }
}
