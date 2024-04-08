namespace FifApi.Models.Products
{
    public class Color
    {
        public string Nom { get; set; }
        public decimal Prix { get; set; }
        public List<Size>? Tailles { get; set; }

    }
}
