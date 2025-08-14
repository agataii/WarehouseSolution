using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Infrastructure.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly IWarehouseDbContext _context;
        public UnitRepository(IWarehouseDbContext context) => _context = context;

        public async Task<IEnumerable<Unit>> GetAllAsync() =>
            await _context.Units.AsNoTracking().ToListAsync();

        public async Task<Unit?> GetByIdAsync(int id) =>
            await _context.Units.FindAsync(id);

        public async Task<Unit> AddAsync(Unit unit)
        {
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();
            return unit;
        }

        public async Task UpdateAsync(Unit unit)
        {
            _context.Units.Update(unit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Unit unit)
        {
            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name) =>
            await _context.Units.AnyAsync(u => u.Name == name);

        public async Task<bool> IsUsedAsync(int unitId)
        {
            return await _context.ReceiptItems.AnyAsync(x => x.UnitId == unitId);
        }
    }
}
