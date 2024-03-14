using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_p_poste_pst")]
    public class Poste
    {
        [Key]
        [Column("pst_id")]
        public int Idposte { get; set; }

        [Required]
        [Column("nomposte")]
        [StringLength(150)]
        public string NomPoste { get; set; }

        [Column("descriptionposte")]
        [StringLength(300)]
        public string? DescriptionPoste { get; set; }

        [InverseProperty("PostePourJoueur")]
        public virtual ICollection<Joueur> JoueurPoste { get; set; } = null!;
    }
}
