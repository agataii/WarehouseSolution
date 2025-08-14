using AutoMapper;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _repository;
        private readonly IMapper _mapper;

        public UnitService(IUnitRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UnitDto>> GetAllAsync()
        {
            var units = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<UnitDto>>(units);
        }

        public async Task<UnitDto> CreateAsync(UnitCreateDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name))
                throw new Exception("Единица измерения с таким именем уже существует");

            var unit = _mapper.Map<Unit>(dto);
            var created = await _repository.AddAsync(unit);
            return _mapper.Map<UnitDto>(created);
        }

        public async Task UpdateAsync(int id, UnitCreateDto dto, bool isActive)
        {
            var unit = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Единица измерения не найдена");

            unit.Name = dto.Name;
            unit.IsActive = isActive;

            await _repository.UpdateAsync(unit);
        }

        public async Task DeleteAsync(int id)
        {
            var unit = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Единица измерения не найдена");

            // Проверяем, используется ли единица измерения в поступлениях
            if (await _repository.IsUsedAsync(id))
                throw new Exception("Невозможно удалить единицу измерения, так как она используется в поступлениях.");

            await _repository.DeleteAsync(unit);
        }
    }
}