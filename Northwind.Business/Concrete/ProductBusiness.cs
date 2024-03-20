using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Northwind.Business.Abstract;
using Northwind.Business.Data.Contexts;
using Northwind.Business.Data.Entities;
using Northwind.Business.Dtos;
using Northwind.Business.Exceptions;
using System.Runtime.Serialization.Formatters;

namespace Northwind.Business.Concrete;

public class ProductBusiness : IProductService
{
    private readonly NorthwindContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductBusiness> _logger;

    public ProductBusiness(NorthwindContext context, IMapper mapper, ILogger<ProductBusiness> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public void Delete(int id)
    {
        var product = _context.Products.SingleOrDefault(t => t.Id == id);
        _context.Products.Remove(product);

        _logger.LogWarning("Ürün Silindi", product);

        _context.SaveChanges();
    }

    public IEnumerable<ProductDto> GetAll()
    {
        var products = _context.Products.AsEnumerable();
        var data = _mapper.Map<IEnumerable<ProductDto>>(products);
        
        _logger.LogInformation("Tüm ürünler getirildi");

        return data;
    }

    public ProductDetailDto GetById(int id)
    {
        var product = _context.Products
            .Include(t => t.Category)
            .SingleOrDefault(t => t.Id == id);

        _logger.LogInformation($"{product.Name} ürünü getirildi");

        return _mapper.Map<ProductDetailDto>(product);
    }

    public IEnumerable<ProductDto> GetListByCategory(int categoryId)
    {
        var products = _context.Products.Where(t => t.CategoryId == categoryId).AsEnumerable();
        var data = _mapper.Map<IEnumerable<ProductDto>>(products);
        return data;
    }

    public ProductDto Insert(CreateProductDto createProductDto)
    {
        var isExist = _context.Products.Any(t => t.Name == createProductDto.Name);
        if (isExist)
            throw new BusinessRuleException($"{createProductDto.Name} ürünü sistemde kayıtlıdır");

        //var product = new Product
        //{
        //    CategoryId = createProductDto.CategoryId,
        //    Name = createProductDto.Name,
        //    QuantityPerUnit = createProductDto.QuantityPerUnit,
        //    UnitPrice = createProductDto.UnitPrice,
        //    UnitsInStock = 1
        //};

        var product = _mapper.Map<Product>(createProductDto);

        _context.Products.Add(product);
        _context.SaveChanges();

        return _mapper.Map<ProductDto>(product);
    }

    public void Update(UpdateProductDto updateProductDto)
    {
        var product = _context.Products.SingleOrDefault(t => t.Id == updateProductDto.Id);

        //product.UnitPrice=updateProductDto.UnitPrice;
        //product.QuantityPerUnit = updateProductDto.Description;
        //product.Id=updateProductDto.Id;
        //product.CategoryId=updateProductDto.CategoryId;
        //product.Name=updateProductDto.Name;
        //product.UnitsInStock = updateProductDto.UnitsInStock;

        _mapper.Map(updateProductDto, product);

        _context.SaveChanges();
    }
}
