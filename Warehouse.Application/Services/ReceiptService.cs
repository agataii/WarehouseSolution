using AutoMapper;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _repository;
        private readonly IMapper _mapper;

        public ReceiptService(IReceiptRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReceiptDto>> GetAllAsync()
        {
            var docs = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReceiptDto>>(docs);
        }

        public async Task<ReceiptDto> CreateAsync(ReceiptCreateDto dto)
        {
            if (await _repository.ExistsByNumberAsync(dto.Number))
                throw new Exception("Документ с таким номером уже существует");

            var doc = _mapper.Map<ReceiptDocument>(dto);
            var created = await _repository.AddAsync(doc);
            return _mapper.Map<ReceiptDto>(created);
        }

        public async Task UpdateAsync(int id, ReceiptCreateDto dto)
        {
            var doc = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Документ не найден");

            doc.Number = dto.Number;
            doc.Date = dto.Date;
            doc.Items = dto.Items.Select(i => new ReceiptItem
            {
                ResourceId = i.ResourceId,
                UnitId = i.UnitId,
                Quantity = i.Quantity
            }).ToList();

            await _repository.UpdateAsync(doc);
        }

        public async Task DeleteAsync(int id)
        {
            var doc = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Документ не найден");

            await _repository.DeleteAsync(doc);
        }

        public async Task<IEnumerable<ReceiptDto>> GetFilteredAsync(ReceiptFilterDto filter)
        {
            var receipts = await GetAllAsync();
            if (filter.DateFrom.HasValue)
                receipts = receipts.Where(r => r.Date >= filter.DateFrom.Value);
            if (filter.DateTo.HasValue)
                receipts = receipts.Where(r => r.Date <= filter.DateTo.Value);
            if (filter.Numbers != null && filter.Numbers.Any())
                receipts = receipts.Where(r => filter.Numbers.Contains(r.Number));
            if (filter.ResourceIds != null && filter.ResourceIds.Any())
                receipts = receipts.Where(r => r.Items.Any(i => filter.ResourceIds.Contains(i.ResourceId)));
            if (filter.UnitIds != null && filter.UnitIds.Any())
                receipts = receipts.Where(r => r.Items.Any(i => filter.UnitIds.Contains(i.UnitId)));
            return receipts;
        }
    }

}