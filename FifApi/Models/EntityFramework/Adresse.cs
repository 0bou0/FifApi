using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifApi.Models.EntityFramework
{
    [Table("t_e_adresse_adr")]
    public class Adresse
    {
        [Key]
        [Column("adr_id")]
        public int IdAdresse { get; set; }

        [Required]
        [Column("adr_codepostal", TypeName ="char(15)")]
        public string CodePostal { get; set; }


        [Column("adr_rue")]
        [StringLength(250)]
        public string? Rue { get; set; }

        [Column("adr_numrue")]
        public int? NumRue { get; set; }

    }
}
