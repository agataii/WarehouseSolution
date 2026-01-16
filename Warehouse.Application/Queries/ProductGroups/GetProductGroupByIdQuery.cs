using MediatR;
using Warehouse.Application.DTOs;

namespace Warehouse.Application.Queries.ProductGroups
{
    public class GetProductGroupByIdQuery : IRequest<ProductGroupDto?>
    {
        public int Id { get; set; }
    }
}
