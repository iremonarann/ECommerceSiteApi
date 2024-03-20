using FluentValidation;
using Northwind.Business.Dtos;

namespace Northwind.Business.Validators;

public class SaveProductDtoValidator<T> : AbstractValidator<T>
    where T : SaveProductDto
{
    public SaveProductDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().Length(5, 250).Must(MustBeGraterThanZero);
        RuleFor(x => x.UnitPrice).GreaterThan(0);
        RuleFor(x => x.CategoryId).NotEmpty();
    }

    private bool MustBeGraterThanZero(string data)
    {
        return true;

        //int value;
        //var result = int.TryParse(data, out value);
        //if (!result)
        //    return false;

        //return value > 0;
    }
}
