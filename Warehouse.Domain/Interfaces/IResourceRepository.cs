using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interfaces
{
    public interface IResourceRepository
    {
        Task<IEnumerable<Resource>> GetAllAsync();
        Task<Resource?> GetByIdAsync(int id);
        Task<Resource> AddAsync(Resource resource);
        Task UpdateAsync(Resource resource);
        Task DeleteAsync(Resource resource);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> IsUsedAsync(int resourceId);
    }
}
