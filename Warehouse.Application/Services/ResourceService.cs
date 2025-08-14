using AutoMapper;
using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _repository;
        private readonly IMapper _mapper;

        public ResourceService(IResourceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResourceDto>> GetAllAsync()
        {
            var resources = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ResourceDto>>(resources);
        }

        public async Task<ResourceDto> CreateAsync(ResourceCreateDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name))
                throw new Exception("Ресурс с таким именем уже существует");

            var resource = _mapper.Map<Resource>(dto);
            var created = await _repository.AddAsync(resource);
            return _mapper.Map<ResourceDto>(created);
        }

        public async Task UpdateAsync(int id, ResourceCreateDto dto, bool isActive)
        {
            var resource = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Ресурс не найден");

            resource.Name = dto.Name;
            resource.IsActive = isActive;

            await _repository.UpdateAsync(resource);
        }

        public async Task DeleteAsync(int id)
        {
            var resource = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Ресурс не найден");

            // Проверяем, используется ли ресурс в поступлениях
            if (await _repository.IsUsedAsync(id))
                throw new Exception("Невозможно удалить ресурс, так как он используется в поступлениях.");

            await _repository.DeleteAsync(resource);
        }
    }
}