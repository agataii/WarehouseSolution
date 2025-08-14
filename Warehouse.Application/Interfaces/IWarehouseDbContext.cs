using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Entities;

namespace Warehouse.Application.Interfaces
{
    public interface IWarehouseDbContext
    {
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<ReceiptDocument> ReceiptDocuments { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
