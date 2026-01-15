using Warehouse.Application.DTOs;

namespace Warehouse.Application.Interfaces
{
    public interface IProductGroupService
    {
        Task<IEnumerable<ProductGroupSummaryDto>> GetAllGroupsAsync();
        Task<ProductGroupDto?> GetGroupByIdAsync(int id);
        Task ProcessUnprocessedProductsAsync();
    }
}
