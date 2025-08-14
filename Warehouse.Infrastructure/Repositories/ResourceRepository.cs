using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Infrastructure.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly IWarehouseDbContext _context;
        public ResourceRepository(IWarehouseDbContext context) => _context = context;

        public async Task<IEnumerable<Resource>> GetAllAsync() =>
            await _context.Resources.AsNoTracking().ToListAsync();

        public async Task<Resource?> GetByIdAsync(int id) =>
            await _context.Resources.FindAsync(id);

        public async Task<Resource> AddAsync(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
            return resource;
        }

        public async Task UpdateAsync(Resource resource)
        {
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Resource resource)
        {
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name) =>
            await _context.Resources.AnyAsync(r => r.Name == name);

        public async Task<bool> IsUsedAsync(int resourceId)
        {
            return await _context.ReceiptItems.AnyAsync(x => x.ResourceId == resourceId);
        }
    }

}