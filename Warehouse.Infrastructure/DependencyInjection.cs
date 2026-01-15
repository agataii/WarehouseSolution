using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Interfaces;
using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Repositories;

namespace Warehouse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Подключение к базе данных SQL Server
        //services.AddDbContext<WarehouseDbContext>(options =>
        //    options.UseSqlServer(configuration.GetConnectionString("Local")));

        // Подключение к базе данных PostgreSQL
        services.AddDbContext<WarehouseDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Neon")));

        // Контекст базы данных
        services.AddScoped<IWarehouseDbContext>(provider => provider.GetRequiredService<WarehouseDbContext>());

        // Репозитории
        services.AddScoped<IResourceRepository, ResourceRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IReceiptRepository, ReceiptRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductGroupRepository, ProductGroupRepository>();

        return services;
    }
}
