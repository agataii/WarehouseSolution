namespace Warehouse.Domain.Entities
{
    public class ProductGroupItem
    {
        public int Id { get; set; }
        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; } // Количество товара в этой группе
        public decimal PricePerUnit { get; set; } // Цена на момент добавления в группу
    }
}
