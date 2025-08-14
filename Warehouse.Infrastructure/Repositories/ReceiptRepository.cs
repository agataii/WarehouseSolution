using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Infrastructure.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly IWarehouseDbContext _context;
        public ReceiptRepository(IWarehouseDbContext context) => _context = context;

        public async Task<IEnumerable<ReceiptDocument>> GetAllAsync() =>
            await _context.ReceiptDocuments
                .Include(r => r.Items)
                .ThenInclude(i => i.Resource)
                .Include(r => r.Items)
                .ThenInclude(i => i.Unit)
                .AsNoTracking()
                .ToListAsync();

        public async Task<ReceiptDocument?> GetByIdAsync(int id) =>
            await _context.ReceiptDocuments
                .Include(r => r.Items)
                .ThenInclude(i => i.Resource)
                .Include(r => r.Items)
                .ThenInclude(i => i.Unit)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<ReceiptDocument> AddAsync(ReceiptDocument doc)
        {
            _context.ReceiptDocuments.Add(doc);
            await _context.SaveChangesAsync();
            return doc;
        }

        public async Task UpdateAsync(ReceiptDocument doc)
        {
            _context.ReceiptDocuments.Update(doc);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ReceiptDocument doc)
        {
            _context.ReceiptDocuments.Remove(doc);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNumberAsync(string number) =>
            await _context.ReceiptDocuments.AnyAsync(r => r.Number == number);
    }
}