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

            // Product
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductCreateDto, Product>();

            // ProductGroup
            CreateMap<ProductGroup, ProductGroupDto>();
            CreateMap<ProductGroup, ProductGroupSummaryDto>();

            // ProductGroupItem
            CreateMap<ProductGroupItem, ProductGroupItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Product.Unit));
        }
    }

}