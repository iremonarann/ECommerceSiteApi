using AutoMapper;
using Northwind.Business.Data.Entities;
using Northwind.Business.Dtos;

namespace Northwind.Business.Profiles;

public class DtoToEntityProfile : Profile
{
    public DtoToEntityProfile()
    {
        CreateMap<CreateProductDto, Product>()
         .ForMember(t => t.QuantityPerUnit, src => src.MapFrom(x => x.Description))
         .ForMember(t => t.UnitsInStock, src => src.MapFrom(x => 1));

        CreateMap<UpdateProductDto, Product>()
            .ForMember(t => t.QuantityPerUnit, src => src.MapFrom(x => x.Description));
    }
}
