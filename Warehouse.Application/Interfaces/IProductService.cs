using Warehouse.Application.DTOs;

namespace Warehouse.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetUnprocessedAsync();
        Task ImportFromExcelAsync(Stream excelStream);
    }
}
