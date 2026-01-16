using MediatR;
using Warehouse.Application.Commands.Products;
using Warehouse.Application.Interfaces;

namespace Warehouse.Application.Handlers.Products
{
    public class ImportProductsFromExcelCommandHandler : IRequestHandler<ImportProductsFromExcelCommand>
    {
        private readonly IProductService _productService;

        public ImportProductsFromExcelCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task Handle(ImportProductsFromExcelCommand request, CancellationToken cancellationToken)
        {
            await _productService.ImportFromExcelAsync(request.ExcelStream);
        }
    }
}
