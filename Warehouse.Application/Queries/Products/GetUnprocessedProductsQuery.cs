using MediatR;
using Warehouse.Application.DTOs;

namespace Warehouse.Application.Queries.Products
{
    public class GetUnprocessedProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
    }
}
