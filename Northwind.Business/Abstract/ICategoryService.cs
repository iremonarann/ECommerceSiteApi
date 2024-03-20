using Northwind.Business.Data.Entities;
using Northwind.Business.Dtos;

namespace Northwind.Business.Abstract;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
}
