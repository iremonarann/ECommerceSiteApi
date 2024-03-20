namespace Northwind.Business.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string QuantityPerUnit { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
}
