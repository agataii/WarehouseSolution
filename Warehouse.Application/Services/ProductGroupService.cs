using AutoMapper;
using Microsoft.Extensions.Logging;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Models;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductGroupRepository _groupRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductGroupService> _logger;
        private const decimal MaxGroupPrice = 200m;

        public ProductGroupService(
            IProductRepository productRepository,
            IProductGroupRepository groupRepository,
            IMapper mapper,
            ILogger<ProductGroupService> logger)
        {
            _productRepository = productRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductGroupSummaryDto>> GetAllGroupsAsync()
        {
            var groups = await _groupRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductGroupSummaryDto>>(groups);
        }

        public async Task<ProductGroupDto?> GetGroupByIdAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            return group == null ? null : _mapper.Map<ProductGroupDto>(group);
        }

        public async Task ProcessUnprocessedProductsAsync()
        {
            var unprocessedProducts = await _productRepository.GetUnprocessedAsync();
            var productsList = unprocessedProducts.ToList();

            if (!productsList.Any())
            {
                _logger.LogInformation("Нет необработанных товаров для группировки");
                return;
            }

            _logger.LogInformation($"Начинаем обработку {productsList.Count} товаров");

            // Преобразуем товары в "единицы" для алгоритма
            var productUnits = new List<ProductUnitModel>();
            foreach (var product in productsList)
            {
                for (int i = 0; i < product.Quantity; i++)
                {
                    productUnits.Add(new ProductUnitModel
                    {
                        ProductId = product.Id,
                        Product = product,
                        Price = product.PricePerUnit
                    });
                }
            }

            // Сортируем по убыванию цены для лучшего результата
            productUnits = productUnits.OrderByDescending(u => u.Price).ToList();

            var groups = new List<ProductGroup>();
            var processedUnits = new HashSet<int>(); // Индексы обработанных единиц

            while (processedUnits.Count < productUnits.Count)
            {
                var nextGroupNumber = await _groupRepository.GetNextGroupNumberAsync();
                var group = new ProductGroup
                {
                    Name = $"Группа {nextGroupNumber}",
                    TotalPrice = 0,
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<ProductGroupItem>()
                };

                // Используем жадный алгоритм с оптимизацией
                var remaining = MaxGroupPrice;
                var groupItems = new Dictionary<int, int>(); // ProductId -> Quantity

                // Пытаемся заполнить группу максимально близко к 200
                for (int i = 0; i < productUnits.Count; i++)
                {
                    if (processedUnits.Contains(i))
                        continue;

                    var unit = productUnits[i];
                    if (unit.Price <= remaining)
                    {
                        if (!groupItems.ContainsKey(unit.ProductId))
                            groupItems[unit.ProductId] = 0;

                        groupItems[unit.ProductId]++;
                        remaining -= unit.Price;
                        processedUnits.Add(i);
                    }
                }

                // Если осталось место, пытаемся добавить более мелкие товары
                if (remaining > 0)
                {
                    for (int i = productUnits.Count - 1; i >= 0; i--)
                    {
                        if (processedUnits.Contains(i))
                            continue;

                        var unit = productUnits[i];
                        if (unit.Price <= remaining)
                        {
                            if (!groupItems.ContainsKey(unit.ProductId))
                                groupItems[unit.ProductId] = 0;

                            groupItems[unit.ProductId]++;
                            remaining -= unit.Price;
                            processedUnits.Add(i);
                        }
                    }
                }

                // Создаем элементы группы
                foreach (var item in groupItems)
                {
                    var product = productsList.First(p => p.Id == item.Key);
                    group.Items.Add(new ProductGroupItem
                    {
                        ProductId = item.Key,
                        Quantity = item.Value,
                        PricePerUnit = product.PricePerUnit
                    });
                    group.TotalPrice += product.PricePerUnit * item.Value;
                }

                if (group.Items.Any())
                {
                    groups.Add(group);
                    await _groupRepository.AddAsync(group);
                    _logger.LogInformation($"Создана {group.Name} с общей ценой {group.TotalPrice:F2} евро");
                }
            }

            // Обновляем статус обработанных товаров
            var productsToUpdate = new List<Product>();
            foreach (var product in productsList)
            {
                var totalUsedInGroups = groups
                    .SelectMany(g => g.Items)
                    .Where(i => i.ProductId == product.Id)
                    .Sum(i => i.Quantity);

                if (totalUsedInGroups >= product.Quantity)
                {
                    product.IsProcessed = true;
                    productsToUpdate.Add(product);
                }
            }

            if (productsToUpdate.Any())
            {
                await _productRepository.UpdateRangeAsync(productsToUpdate);
            }

            _logger.LogInformation($"Обработка завершена. Создано {groups.Count} групп");
        }
    }
}