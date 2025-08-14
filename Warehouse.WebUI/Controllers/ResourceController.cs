using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;

namespace Warehouse.WebUI.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IResourceService _service;

        public ResourceController(IResourceService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var resources = await _service.GetAllAsync();
            return View(resources);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ResourceCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ResourceCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var resources = await _service.GetAllAsync();
            var resource = resources.FirstOrDefault(r => r.Id == id);
            if (resource == null) return NotFound();

            return View(new ResourceCreateDto { Name = resource.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ResourceCreateDto dto, bool isActive)
        {
            if (!ModelState.IsValid) return View(dto);

            await _service.UpdateAsync(id, dto, isActive);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}