using AutoMapper;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository repository,
            IMapper mapper,
            ILogger<ProductService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetUnprocessedAsync()
        {
            var products = await _repository.GetUnprocessedAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task ImportFromExcelAsync(Stream excelStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(excelStream);
            var worksheet = package.Workbook.Worksheets[0];

            if (worksheet == null)
                throw new Exception("Лист не найден в файле Excel");

            var products = new List<Product>();
            var startRow = 2; // Пропускаем заголовок

            for (int row = startRow; row <= worksheet.Dimension?.End.Row; row++)
            {
                var name = worksheet.Cells[row, 1].Value?.ToString();
                if (string.IsNullOrWhiteSpace(name))
                    break; // Конец данных

                var unit = worksheet.Cells[row, 2].Value?.ToString() ?? "";
                var pricePerUnitStr = worksheet.Cells[row, 3].Value?.ToString();
                var quantityStr = worksheet.Cells[row, 4].Value?.ToString();

                if (!decimal.TryParse(pricePerUnitStr?.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal pricePerUnit))
                {
                    _logger.LogWarning($"Не удалось распарсить цену в строке {row}: {pricePerUnitStr}");
                    continue;
                }

                if (!int.TryParse(quantityStr, out int quantity))
                {
                    _logger.LogWarning($"Не удалось распарсить количество в строке {row}: {quantityStr}");
                    continue;
                }

                products.Add(new Product
                {
                    Name = name.Trim(),
                    Unit = unit.Trim(),
                    PricePerUnit = pricePerUnit,
                    Quantity = quantity,
                    IsProcessed = false,
                    CreatedAt = DateTime.UtcNow
                });
            }

            if (products.Any())
            {
                await _repository.AddRangeAsync(products);
                _logger.LogInformation($"Импортировано {products.Count} товаров из Excel");
            }
        }
    }
}