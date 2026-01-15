using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;

namespace Warehouse.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductGroupsController : ControllerBase
    {
        private readonly IProductGroupService _productGroupService;
        private readonly ILogger<ProductGroupsController> _logger;

        public ProductGroupsController(
            IProductGroupService productGroupService,
            ILogger<ProductGroupsController> logger)
        {
            _productGroupService = productGroupService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductGroupSummaryDto>>> GetAllGroups()
        {
            var groups = await _productGroupService.GetAllGroupsAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductGroupDto>> GetGroupById(int id)
        {
            var group = await _productGroupService.GetGroupByIdAsync(id);
            if (group == null)
                return NotFound();

            return Ok(group);
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessProducts()
        {
            try
            {
                await _productGroupService.ProcessUnprocessedProductsAsync();
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