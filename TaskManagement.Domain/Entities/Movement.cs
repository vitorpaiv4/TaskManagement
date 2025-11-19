namespace TaskManagement.Domain.Entities
{
    public class Movement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } = null!;
    }
}
