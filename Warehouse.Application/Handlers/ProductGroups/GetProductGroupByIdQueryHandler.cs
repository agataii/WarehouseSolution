using MediatR;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Queries.ProductGroups;

namespace Warehouse.Application.Handlers.ProductGroups
{
    public class GetProductGroupByIdQueryHandler : IRequestHandler<GetProductGroupByIdQuery, ProductGroupDto?>
    {
        private readonly IProductGroupService _productGroupService;

        public GetProductGroupByIdQueryHandler(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        public async Task<ProductGroupDto?> Handle(GetProductGroupByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productGroupService.GetGroupByIdAsync(request.Id);
        }
    }
}
