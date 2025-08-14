using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;

namespace Warehouse.Infrastructure.Data
{
    public class WarehouseDbContext : DbContext, IWarehouseDbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<ReceiptDocument> ReceiptDocuments { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Resource>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<Unit>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<ReceiptDocument>()
                .HasIndex(d => d.Number)
                .IsUnique();
        }
    }
}
