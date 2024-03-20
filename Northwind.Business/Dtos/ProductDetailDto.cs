namespace Northwind.Business.Dtos;

public class ProductDetailDto : ProductDto
{
    public CategoryDto Category { get; set; }
}
