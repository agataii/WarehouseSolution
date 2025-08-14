using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interfaces
{
    public interface IReceiptRepository
    {
        Task<IEnumerable<ReceiptDocument>> GetAllAsync();
        Task<ReceiptDocument?> GetByIdAsync(int id);
        Task<ReceiptDocument> AddAsync(ReceiptDocument doc);
        Task UpdateAsync(ReceiptDocument doc);
        Task DeleteAsync(ReceiptDocument doc);
        Task<bool> ExistsByNumberAsync(string number);
    }
}
