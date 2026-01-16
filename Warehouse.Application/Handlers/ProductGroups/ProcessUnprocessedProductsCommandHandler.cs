using MediatR;
using Warehouse.Application.Commands.ProductGroups;
using Warehouse.Application.Interfaces;

namespace Warehouse.Application.Handlers.ProductGroups
{
    public class ProcessUnprocessedProductsCommandHandler : IRequestHandler<ProcessUnprocessedProductsCommand>
    {
        private readonly IProductGroupService _productGroupService;

        public ProcessUnprocessedProductsCommandHandler(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        public async Task Handle(ProcessUnprocessedProductsCommand request, CancellationToken cancellationToken)
        {
            await _productGroupService.ProcessUnprocessedProductsAsync();
        }
    }
}
