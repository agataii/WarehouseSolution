using MediatR;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Queries.ProductGroups;

namespace Warehouse.Application.Handlers.ProductGroups
{
    public class GetAllProductGroupsQueryHandler : IRequestHandler<GetAllProductGroupsQuery, IEnumerable<ProductGroupSummaryDto>>
    {
        private readonly IProductGroupService _productGroupService;

        public GetAllProductGroupsQueryHandler(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        public async Task<IEnumerable<ProductGroupSummaryDto>> Handle(GetAllProductGroupsQuery request, CancellationToken cancellationToken)
        {
            return await _productGroupService.GetAllGroupsAsync();
        }
    }
}
