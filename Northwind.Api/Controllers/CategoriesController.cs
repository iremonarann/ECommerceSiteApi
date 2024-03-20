using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Northwind.Business.Abstract;
using Northwind.Business.Concrete;

namespace Northwind.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public CategoriesController(ICategoryService categoryService, IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var data = await _categoryService.GetAllAsync();

        return Ok(data);
    }

    [HttpGet("{id:int}/products")]
    public IActionResult GetByCategory(int id)
    {
        var res = _productService.GetListByCategory(id);

        return Ok(res);
    }



}
