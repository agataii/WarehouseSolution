using AutoMapper;
using Warehouse.Application.DTOs;
using Warehouse.Domain.Entities;

namespace Warehouse.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Resource
            CreateMap<Resource, ResourceDto>().ReverseMap();
            CreateMap<ResourceCreateDto, Resource>();

            // Unit
            CreateMap<Unit, UnitDto>().ReverseMap();
            CreateMap<UnitCreateDto, Unit>();

            // ReceiptDocument
            CreateMap<ReceiptDocument, ReceiptDto>().ReverseMap();
            CreateMap<ReceiptCreateDto, ReceiptDocument>();

            // ReceiptItem
            CreateMap<ReceiptItem, ReceiptItemDto>()
                .ForMember(dest => dest.ResourceName, opt => opt.MapFrom(src => src.Resource.Name))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name));
            CreateMap<ReceiptItemCreateDto, ReceiptItem>();
        }
    }

}