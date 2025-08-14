using Warehouse.Application.DTOs;
using Warehouse.Domain.Entities;

namespace Warehouse.Application.Interfaces
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitDto>> GetAllAsync();
        Task<UnitDto> CreateAsync(UnitCreateDto dto);
        Task UpdateAsync(int id, UnitCreateDto dto, bool isActive);
        Task DeleteAsync(int id);
    }
}
