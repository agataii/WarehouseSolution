using Warehouse.Domain.Entities;

namespace Warehouse.Application.Models
{
    public class ProductUnitModel
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
