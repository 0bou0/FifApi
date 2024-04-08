
using System.Runtime.CompilerServices;

namespace FifApi.Models.Products
{
    public class Product
    {
        public string NomProduit { get; set; }
        public string DescriptionProduit {  get; set; }
        public string Image { get; set; }
        public int Marque { get; set; }
        public string Nation { get; set; }
        public int Categorie { get; set; }
        public List<Color>? Couleurs { get; set; }
    }
}
