using MediatR;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Queries.Products;

namespace Warehouse.Application.Handlers.Products
{
    public class GetUnprocessedProductsQueryHandler : IRequestHandler<GetUnprocessedProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductService _productService;

        public GetUnprocessedProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetUnprocessedProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetUnprocessedAsync();
        }
    }
}
