using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.Commands.ProductGroups;
using Warehouse.Application.DTOs;
using Warehouse.Application.Queries.ProductGroups;

namespace Warehouse.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductGroupsController> _logger;

        public ProductGroupsController(
            IMediator mediator,
            ILogger<ProductGroupsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductGroupSummaryDto>>> GetAllGroups()
        {
            var query = new GetAllProductGroupsQuery();
            var groups = await _mediator.Send(query);
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductGroupDto>> GetGroupById(int id)
        {
            var query = new GetProductGroupByIdQuery { Id = id };
            var group = await _mediator.Send(query);
            if (group == null)
                return NotFound();

            return Ok(group);
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessProducts()
        {
            try
            {
                var command = new ProcessUnprocessedProductsCommand();
                await _mediator.Send(command);
                return Ok(new { message = "Обработка товаров запущена" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке товаров");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}