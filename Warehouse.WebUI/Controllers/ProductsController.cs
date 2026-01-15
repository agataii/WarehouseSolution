using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;

namespace Warehouse.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не предоставлен");

            if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Файл должен иметь расширение .xlsx");

            try
            {
                using var stream = file.OpenReadStream();
                await _productService.ImportFromExcelAsync(stream);
                return Ok(new { message = "Товары успешно загружены" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке файла Excel");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("unprocessed")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetUnprocessed()
        {
            var products = await _productService.GetUnprocessedAsync();
            return Ok(products);
        }
    }
}