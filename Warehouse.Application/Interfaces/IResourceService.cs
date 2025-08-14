using Warehouse.Application.DTOs;
using Warehouse.Domain.Entities;

namespace Warehouse.Application.Interfaces
{
    public interface IResourceService
    {
        Task<IEnumerable<ResourceDto>> GetAllAsync();
        Task<ResourceDto> CreateAsync(ResourceCreateDto dto);
        Task UpdateAsync(int id, ResourceCreateDto dto, bool isActive);
        Task DeleteAsync(int id);
    }
}
