using System.ComponentModel.DataAnnotations;

namespace Northwind.Business.Dtos;

public abstract class SaveProductDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal UnitPrice { get; set; }
}