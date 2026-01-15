namespace Warehouse.Application.DTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal PricePerUnit { get; set; }
        public int Quantity { get; set; }
    }
}
