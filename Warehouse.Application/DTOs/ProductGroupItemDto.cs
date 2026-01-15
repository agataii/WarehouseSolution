namespace Warehouse.Application.DTOs
{
    public class ProductGroupItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal PricePerUnit { get; set; }
        public int Quantity { get; set; }
    }
}
