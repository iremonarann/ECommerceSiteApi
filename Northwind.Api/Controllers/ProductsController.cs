using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Abstract;
using Northwind.Business.Data.Entities;
using Northwind.Business.Dtos;

namespace Northwind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] //Kimlik Doğrulama kontrolü tüm aksiyonlar için geçerli olur
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous] //Kimlik doğrulaması gerektirmez (Anonim girişlere izin ver)
        public IActionResult Get()
        {
            var res = _productService.GetAll();
            return Ok(res);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var res = _productService.GetById(id);
            return Ok(res);
        }

        [HttpPost]
        [Authorize("HasGmailAddress")]
        public IActionResult Post([FromBody] CreateProductDto product)
        {
            var res = _productService.Insert(product);

            return Created(string.Empty, res);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] UpdateProductDto product)
        {
            product.Id = id;
            _productService.Update(product);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                _productService.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(title: "Üzgünüz! Sistemde beklenmedik bir hata oluştu", statusCode: 500);
            }
        }
    }
}
