using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;

namespace Warehouse.Infrastructure.Data
{
    public class WarehouseDbContext : DbContext, IWarehouseDbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<ReceiptDocument> ReceiptDocuments { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ProductGroupItem> ProductGroupItems { get; set; }

        public override int SaveChanges()
        {
            ConvertDateTimeToUtc();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDateTimeToUtc();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ConvertDateTimeToUtc()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.CurrentValue == null)
                        continue;

                    var propertyType = property.Metadata.ClrType;
                    if ((propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)) 
                        && property.CurrentValue is DateTime dateTime 
                        && dateTime.Kind != DateTimeKind.Utc)
                    {
                        property.CurrentValue = dateTime.Kind == DateTimeKind.Unspecified
                            ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
                            : dateTime.ToUniversalTime();
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Value converter для автоматической конвертации DateTime в UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc 
                    ? v 
                    : (v.Kind == DateTimeKind.Unspecified 
                        ? DateTime.SpecifyKind(v, DateTimeKind.Utc) 
                        : v.ToUniversalTime()),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue 
                    ? (v.Value.Kind == DateTimeKind.Utc 
                        ? v.Value 
                        : (v.Value.Kind == DateTimeKind.Unspecified 
                            ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) 
                            : v.Value.ToUniversalTime()))
                    : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            // Применяем конвертер ко всем DateTime свойствам
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }

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
