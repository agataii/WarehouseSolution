using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interfaces
{
    public interface IProductGroupRepository
    {
        Task<IEnumerable<ProductGroup>> GetAllAsync();
        Task<ProductGroup?> GetByIdAsync(int id);
        Task<ProductGroup> AddAsync(ProductGroup group);
        Task<int> GetNextGroupNumberAsync();
    }
}
