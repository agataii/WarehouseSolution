using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;

namespace Warehouse.WebUI.Controllers
{
    public class UnitController : Controller
    {
        private readonly IUnitService _service;

        public UnitController(IUnitService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var units = await _service.GetAllAsync();
            return View(units);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UnitCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UnitCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var units = await _service.GetAllAsync();
            var unit = units.FirstOrDefault(r => r.Id == id);
            if (unit == null) return NotFound();

            return View(new UnitCreateDto { Name = unit.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UnitCreateDto dto, bool isActive)
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