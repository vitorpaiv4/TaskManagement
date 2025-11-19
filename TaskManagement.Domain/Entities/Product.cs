namespace TaskManagement.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Movement> Movements { get; set; } = new List<Movement>();
    }
}
