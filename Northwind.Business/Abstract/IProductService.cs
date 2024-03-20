using Northwind.Business.Data.Entities;
using Northwind.Business.Dtos;

namespace Northwind.Business.Abstract;

public interface IProductService
{
    IEnumerable<ProductDto> GetAll();

    IEnumerable<ProductDto> GetListByCategory(int categoryId);

    ProductDetailDto GetById(int id);

    ProductDto Insert(CreateProductDto product);

    void Update(UpdateProductDto product);

    void Delete(int id);
}


