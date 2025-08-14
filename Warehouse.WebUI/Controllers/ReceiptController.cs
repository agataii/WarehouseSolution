using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.WebUI.Models;

namespace Warehouse.WebUI.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly IReceiptService _receiptService;
        private readonly IResourceService _resourceService;
        private readonly IUnitService _unitService;

        public ReceiptController(IReceiptService receiptService, IResourceService resourceService, IUnitService unitService)
        {
            _receiptService = receiptService;
            _resourceService = resourceService;
            _unitService = unitService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ReceiptFilterDto filter)
        {
            var receipts = await _receiptService.GetFilteredAsync(filter ?? new ReceiptFilterDto());
            var allReceipts = await _receiptService.GetAllAsync();

            var model = new ReceiptIndexViewModel
            {
                Receipts = receipts,
                Filter = filter ?? new ReceiptFilterDto(),
                Numbers = allReceipts.Select(r => r.Number).Distinct().OrderBy(n => n).ToList(),
                Resources = await GetResourceSelectListAsync(filter?.ResourceIds ?? []),
                Units = await GetUnitSelectListAsync(filter?.UnitIds ?? [])
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Resources = await GetResourceSelectListAsync(Array.Empty<int>());
            ViewBag.Units = await GetUnitSelectListAsync(Array.Empty<int>());

            return View(new ReceiptCreateDto
            {
                Date = DateTime.Now,
                Items = new List<ReceiptItemCreateDto> ()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReceiptCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Resources = await GetResourceSelectListAsync(Array.Empty<int>());
                ViewBag.Units = await GetUnitSelectListAsync(Array.Empty<int>());

                return View(dto);
            }

            await _receiptService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var receipts = await _receiptService.GetAllAsync();
            var receipt = receipts.FirstOrDefault(r => r.Id == id);
            if (receipt == null) return NotFound();

            var selectedResourceIds = receipt.Items.Select(i => i.ResourceId).ToList();
            var selectedUnitIds = receipt.Items.Select(i => i.UnitId).ToList();

            ViewBag.Resources = await GetResourceSelectListAsync(selectedResourceIds);
            ViewBag.Units = await GetUnitSelectListAsync(selectedUnitIds);

            return View(new ReceiptCreateDto
            {
                Number = receipt.Number,
                Date = receipt.Date,
                Items = receipt.Items.Select(i => new ReceiptItemCreateDto
                {
                    ResourceId = i.ResourceId,
                    UnitId = i.UnitId,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReceiptCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var selectedResourceIds = dto.Items.Select(i => i.ResourceId).ToList();
                var selectedUnitIds = dto.Items.Select(i => i.UnitId).ToList();

                ViewBag.Resources = await GetResourceSelectListAsync(selectedResourceIds);
                ViewBag.Units = await GetUnitSelectListAsync(selectedUnitIds);

                return View(dto);
            }

            await _receiptService.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _receiptService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Метод для формирования SelectList ресурсов
        private async Task<SelectList> GetResourceSelectListAsync(IEnumerable<int> selectedResourceIds)
        {
            var allResources = await _resourceService.GetAllAsync();
            var activeResources = allResources.Where(r => r.IsActive).ToList();

            // Добавляем неактивные, если они уже выбраны в документе
            var selectedInactive = allResources
                .Where(r => !r.IsActive && selectedResourceIds.Contains(r.Id));

            var result = activeResources
                .Concat(selectedInactive)
                .DistinctBy(r => r.Id)
                .OrderBy(r => r.Name)
                .ToList();

            return new SelectList(result, "Id", "Name");
        }

        // Метод для формирования SelectList единиц измерения
        private async Task<SelectList> GetUnitSelectListAsync(IEnumerable<int> selectedUnitIds)
        {
            var allUnits = await _unitService.GetAllAsync();
            var activeUnits = allUnits.Where(u => u.IsActive).ToList();

            var selectedInactive = allUnits
                .Where(u => !u.IsActive && selectedUnitIds.Contains(u.Id));

            var result = activeUnits
                .Concat(selectedInactive)
                .DistinctBy(u => u.Id)
                .OrderBy(u => u.Name)
                .ToList();

            return new SelectList(result, "Id", "Name");
        }
    }

}