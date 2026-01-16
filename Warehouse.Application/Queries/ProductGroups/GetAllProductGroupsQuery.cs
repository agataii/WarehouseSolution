using MediatR;
using Warehouse.Application.DTOs;

namespace Warehouse.Application.Queries.ProductGroups
{
    public class GetAllProductGroupsQuery : IRequest<IEnumerable<ProductGroupSummaryDto>>
    {
    }
}
