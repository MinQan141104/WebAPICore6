namespace MyApiNetCore6.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
