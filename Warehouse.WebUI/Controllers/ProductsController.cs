using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.Commands.Products;
using Warehouse.Application.DTOs;
using Warehouse.Application.Queries.Products;

namespace Warehouse.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IMediator mediator,
            ILogger<ProductsController> logger)
        {
            _mediator = mediator;
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
                var command = new ImportProductsFromExcelCommand
                {
                    ExcelStream = stream
                };
                await _mediator.Send(command);
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
            var query = new GetUnprocessedProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }
    }
}