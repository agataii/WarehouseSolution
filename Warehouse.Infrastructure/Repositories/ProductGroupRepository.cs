using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Infrastructure.Repositories
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        private readonly IWarehouseDbContext _context;

        public ProductGroupRepository(IWarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductGroup>> GetAllAsync()
        {
            return await _context.ProductGroups
                .Include(g => g.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(g => g.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductGroup?> GetByIdAsync(int id)
        {
            return await _context.ProductGroups
                .Include(g => g.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<ProductGroup> AddAsync(ProductGroup group)
        {
            _context.ProductGroups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<int> GetNextGroupNumberAsync()
        {
            var lastGroup = await _context.ProductGroups
                .OrderByDescending(g => g.CreatedAt)
                .FirstOrDefaultAsync();

            if (lastGroup == null) return 1;

            // Извлекаем номер из названия "Группа 1", "Группа 2" и т.д.
            var match = System.Text.RegularExpressions.Regex.Match(lastGroup.Name, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int lastNumber))
            {
                return lastNumber + 1;
            }

            return 1;
        }
    }
}
