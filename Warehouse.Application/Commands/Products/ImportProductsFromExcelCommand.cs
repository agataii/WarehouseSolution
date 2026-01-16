using MediatR;

namespace Warehouse.Application.Commands.Products
{
    public class ImportProductsFromExcelCommand : IRequest
    {
        public Stream ExcelStream { get; set; } = null!;
    }
}
