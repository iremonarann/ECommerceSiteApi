using AutoMapper;
using Northwind.Business.Data.Entities;
using Northwind.Business.Dtos;

namespace Northwind.Business.Profiles;

public class EntityToDtoProfile : Profile
{
    public EntityToDtoProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductDetailDto>();

        CreateMap<Category, CategoryDto>();
    }
}
