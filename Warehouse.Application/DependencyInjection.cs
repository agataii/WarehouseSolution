using Microsoft.Extensions.DependencyInjection;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Mapping;
using Warehouse.Application.Services;

namespace Warehouse.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Сервисы
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IReceiptService, ReceiptService>();

            // AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            return services;
        }
    }
}
