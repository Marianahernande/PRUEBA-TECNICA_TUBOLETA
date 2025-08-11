using AutoMapper;
using ECommerce.Application.Products;
using ECommerce.Domain.Entities;

namespace ECommerce.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForCtorParam("Category", opt => opt.MapFrom(src => src.Category!.Name));
    }
}