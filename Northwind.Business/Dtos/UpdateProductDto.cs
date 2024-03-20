namespace Northwind.Business.Dtos;

public class UpdateProductDto : SaveProductDto
{
    public int Id { get; set; }
    public short UnitsInStock { get; set; }
}