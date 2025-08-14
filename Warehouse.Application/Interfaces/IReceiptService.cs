using Warehouse.Application.DTOs;

namespace Warehouse.Application.Interfaces
{
    public interface IReceiptService
    {
        Task<IEnumerable<ReceiptDto>> GetAllAsync();
        Task<ReceiptDto> CreateAsync(ReceiptCreateDto dto);
        Task UpdateAsync(int id, ReceiptCreateDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<ReceiptDto>> GetFilteredAsync(ReceiptFilterDto filter);
    }
}
