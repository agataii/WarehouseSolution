using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetUnprocessedAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task AddRangeAsync(IEnumerable<Product> products);
        Task UpdateAsync(Product product);
        Task UpdateRangeAsync(IEnumerable<Product> products);
    }
}
