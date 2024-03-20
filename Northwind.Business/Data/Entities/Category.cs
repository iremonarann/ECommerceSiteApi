namespace Northwind.Business.Data.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public byte[]? Picture { get; set; }

    public ICollection<Product>? Products { get; set; }
}
