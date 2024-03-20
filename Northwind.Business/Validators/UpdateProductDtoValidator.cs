using FluentValidation;
using Northwind.Business.Dtos;

namespace Northwind.Business.Validators;

public class UpdateProductDtoValidator : SaveProductDtoValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UnitsInStock).GreaterThan(default(short));
    }
}
