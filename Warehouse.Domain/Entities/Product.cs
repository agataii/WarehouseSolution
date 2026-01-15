namespace Warehouse.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal PricePerUnit { get; set; }
        public int Quantity { get; set; }
        public bool IsProcessed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProductGroupItem> GroupItems { get; set; } = new List<ProductGroupItem>();
    }
}
