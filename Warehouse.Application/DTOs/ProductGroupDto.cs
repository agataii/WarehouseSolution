namespace Warehouse.Application.DTOs
{
    public class ProductGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProductGroupItemDto> Items { get; set; } = new();
    }
}
