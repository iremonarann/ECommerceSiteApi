using Microsoft.EntityFrameworkCore;
using Northwind.Business.Data.Entities;

namespace Northwind.Business.Data.Contexts;

public class NorthwindContext : DbContext
{
    public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().Property(t => t.Id).HasColumnName("CategoryID");
        modelBuilder.Entity<Category>().Property(t => t.Name).HasColumnName("CategoryName");

        modelBuilder.Entity<Product>().Property(t => t.Id).HasColumnName("ProductID");
        modelBuilder.Entity<Product>().Property(t => t.Name).HasColumnName("ProductName");

        base.OnModelCreating(modelBuilder);
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    base.OnConfiguring(optionsBuilder);
    //}
}
