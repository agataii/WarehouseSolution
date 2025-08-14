using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interfaces
{
    public interface IUnitRepository
    {
        Task<IEnumerable<Unit>> GetAllAsync();
        Task<Unit?> GetByIdAsync(int id);
        Task<Unit> AddAsync(Unit unit);
        Task UpdateAsync(Unit unit);
        Task DeleteAsync(Unit unit);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> IsUsedAsync(int unitId);
    }
}
